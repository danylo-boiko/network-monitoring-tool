import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { ApolloError, ApolloQueryResult } from "@apollo/client/core";
import { PacketsService } from "../../../graphql/services/packets.service";
import { ToasterService } from "../../../../core/services/toaster.service";
import { MatDialog } from "@angular/material/dialog";
import { UpdateChartSettingsComponent } from '../../dialogs/update-chart-settings/update-chart-settings.component';
import { ChartSettingsService } from "../../services/chart-settings.service";
import {
  DateRangeMode,
  DeviceDto,
  GetPacketsChartDataByDeviceIdQuery,
  PacketsChartDataDto
} from "../../../graphql/services/graphql.service";

@UntilDestroy()
@Component({
  selector: 'app-packets-chart',
  templateUrl: './packets-chart.component.html',
  styleUrls: ['./packets-chart.component.scss']
})
export class PacketsChartComponent implements OnInit, OnDestroy {
  @Input() devices!: DeviceDto[];

  public packetsChart!: ApexCharts;
  public deviceSelected: boolean = false;

  constructor(
    private readonly _packetsService: PacketsService,
    private readonly _toasterService: ToasterService,
    private readonly _chartSettingsService: ChartSettingsService,
    private readonly _dialog: MatDialog) {
  }

  public ngOnInit(): void {
    this.refreshPacketsChart();
  }

  public ngOnDestroy(): void {
    this.packetsChart?.destroy();
  }

  public updateChartSettings(): void {
    const dialogRef = this._dialog.open(UpdateChartSettingsComponent, {
      data: {
        devices: this.devices
      }
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(response => response?.modified)
      )
      .subscribe(() => {
        this.refreshPacketsChart();
      });
  }

  public refreshPacketsChart(): void {
    const chartSettings = this._chartSettingsService.getChartSettings();
    if (!chartSettings || this.devices.findIndex(d => d.id == chartSettings?.deviceId) == -1) {
      return;
    }

    this._packetsService
      .getPacketsChartDataByDeviceId(chartSettings)
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<ApolloQueryResult<GetPacketsChartDataByDeviceIdQuery>>response).data!.packetsChartDataByDeviceId)
      )
      .subscribe({
        next: (packetsChartData: PacketsChartDataDto) => {
          this.deviceSelected = true;

          if (this.packetsChart) {
            this.packetsChart.destroy();
          }

          const chartOptions = this.getChartOptions(packetsChartData, chartSettings.dateRangeMode);
          this.packetsChart = new ApexCharts(document.querySelector('#packets-chart'), chartOptions);
          this.packetsChart?.render();
        },
        error: (error: ApolloError) => {
          this._toasterService.showError(error.message);
        }
      });
  }

  private getChartOptions(packetsChartData: PacketsChartDataDto, dateRangeMode: DateRangeMode) {
    const series = packetsChartData.series.map((el) => ({
      name: el.key,
      data: el.value
    }));

    return {
      chart: {
        height: 380,
        type: 'area',
        toolbar: false
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: 'smooth'
      },
      series: series,
      xaxis: {
        type: 'datetime',
        categories: packetsChartData.categories,
        labels: {
          datetimeUTC: false
        }
      },
      tooltip: {
        x: {
          format: dateRangeMode == DateRangeMode.Day ? 'dd/MM/yy HH:mm' : 'dd/MM/yy'
        }
      },
      legend: {
        position: 'top',
        horizontalAlign: 'right'
      }
    };
  }
}
