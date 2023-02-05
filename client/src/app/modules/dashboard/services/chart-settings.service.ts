import { Injectable } from '@angular/core';
import { DateRangeMode } from '../../graphql/services/graphql.service';
import { ChartSettings } from '../models/chart-settings.model';

@Injectable({
  providedIn: 'root'
})
export class ChartSettingsService {
  private readonly chartSettingsKey: string = 'chart-settings';

  constructor() {
  }

  public getChartSettings(): ChartSettings | null {
    const str = localStorage.getItem(this.chartSettingsKey);

    if (!str) {
      return null;
    }

    const raw = JSON.parse(str);

    return new ChartSettings(raw.deviceId, raw.dateRangeMode);
  }

  public getDefaultChartSettings(): ChartSettings {
    return new ChartSettings("", DateRangeMode.Day);
  }

  public updateChartSettings(chartSettings: ChartSettings): void {
    localStorage.setItem(this.chartSettingsKey, JSON.stringify(chartSettings));
  }
}
