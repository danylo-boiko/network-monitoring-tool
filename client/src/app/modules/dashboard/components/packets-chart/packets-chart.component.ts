import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { DeviceDto, GetPacketsByDeviceIdQuery, PacketDto } from "../../../graphql/services/graphql.service";
import { DateRangeMode } from "../../enums/date-range-mode.enum";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { ApolloError, ApolloQueryResult } from "@apollo/client/core";
import { PacketsService } from "../../../graphql/services/packets.service";
import { ToasterService } from "../../../../core/services/toaster.service";
import { MatDialog } from "@angular/material/dialog";
import { UpdateChartSettingsComponent } from '../../dialogs/update-chart-settings/update-chart-settings.component';
import { ChartSettingsService } from "../../services/chart-settings.service";

@UntilDestroy()
@Component({
  selector: 'app-packets-chart',
  templateUrl: './packets-chart.component.html',
  styleUrls: ['./packets-chart.component.scss']
})
export class PacketsChartComponent implements OnInit, OnDestroy {
  @Input() devices!: DeviceDto[];

  public packets!: PacketDto[];
  public packetsChart!: ApexCharts;
  public dateRangeMode = DateRangeMode.Day;

  constructor(
    private readonly _packetsService: PacketsService,
    private readonly _toasterService: ToasterService,
    private readonly _chartSettingsService: ChartSettingsService,
    private readonly _dialog: MatDialog) {
  }

  public ngOnInit(): void {
    const chartSettings = this._chartSettingsService.getChartSettings();
    if (chartSettings && this.devices.findIndex(d => d.id == chartSettings?.deviceId) != -1) {
      this.getPackets(chartSettings!.deviceId);
    }
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
        const chartSettings = this._chartSettingsService.getChartSettings();
        if (chartSettings) {
          this.getPackets(chartSettings!.deviceId);
        }
      });
  }

  private getPackets(deviceId: string): void {
    this._packetsService
      .getPacketsByDeviceId({
        deviceId
      })
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<ApolloQueryResult<GetPacketsByDeviceIdQuery>>response).data!.packetsByDeviceId)
      )
      .subscribe({
        next: (packets: Array<PacketDto>) => {
          this.packets = packets;
          this.refreshChart();
        },
        error: (error: ApolloError) => {
          this._toasterService.showError(error.message);
        }
      });
  }

  private refreshChart(): void {
    if (this.packetsChart) {
      this.packetsChart.destroy();
    }

    this.packetsChart = new ApexCharts(document.querySelector('#packets-chart'), this.getChartOptions());
    this.packetsChart?.render();
  }

  private getChartOptions() {
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
      series: [
        {
          name: 'Passed',
          data: [31, 40, 28]
        },
        {
          name: 'Dropped',
          data: [11, 32, 45]
        },
      ],
      xaxis: {
        type: 'datetime',
        categories: [
          '2019-11-24T00:00:00',
          '2019-11-24T01:30:00',
          '2019-11-24T02:30:00'
        ],
      },
      tooltip: {
        x: {
          format: 'dd/MM/yy HH:mm'
        }
      },
      legend: {
        position: 'top',
        horizontalAlign: 'right'
      }
    };
  }
}
