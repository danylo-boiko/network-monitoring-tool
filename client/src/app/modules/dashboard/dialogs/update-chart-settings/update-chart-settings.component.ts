import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { DateRangeMode, DeviceDto } from "../../../graphql/services/graphql.service";
import { ChartSettingsService } from "../../services/chart-settings.service";
import { FormControl, FormGroup } from "@angular/forms";
import { UpdateChartSettingsForm } from "./update-chart-settings.form";
import { ChartSettings } from '../../models/chart-settings.model';

@Component({
  selector: 'app-update-chart-settings',
  templateUrl: './update-chart-settings.component.html',
  styleUrls: ['./update-chart-settings.component.scss']
})
export class UpdateChartSettingsComponent implements OnInit {
  public updateChartSettingsForm!: FormGroup<UpdateChartSettingsForm>;
  public allowedDateRangeModes = Object.values(DateRangeMode);
  private chartSettings!: ChartSettings | null;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { devices: DeviceDto[] },
    private readonly _dialogRef: MatDialogRef<UpdateChartSettingsComponent>,
    private readonly _chartSettingsService: ChartSettingsService) {
  }

  public ngOnInit(): void {
    this.chartSettings = this._chartSettingsService.getChartSettings();

    if (!this.chartSettings || this.data.devices.findIndex(d => d.id == this.chartSettings?.deviceId) == -1) {
      this.chartSettings = this._chartSettingsService.getDefaultChartSettings();
    }

    this.updateChartSettingsForm = new FormGroup<UpdateChartSettingsForm>({
      deviceId: new FormControl<string>(this.chartSettings.deviceId, {
        nonNullable: true
      }),
      dateRangeMode: new FormControl<DateRangeMode>(this.chartSettings.dateRangeMode, {
        nonNullable: true
      })
    });
  }

  public updateChartSettings(): void {
    const raw = this.updateChartSettingsForm.getRawValue();
    const chartSettings = new ChartSettings(raw.deviceId, raw.dateRangeMode);

    this._chartSettingsService.updateChartSettings(chartSettings);

    this._dialogRef.close({
      modified: !this.chartSettings?.equals(chartSettings)
    });
  }

  public cancel(): void {
    this._dialogRef.close();
  }
}
