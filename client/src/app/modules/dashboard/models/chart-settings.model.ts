import { DateRangeMode } from "../enums/date-range-mode.enum";

export class ChartSettings {
  public deviceId: string;
  public dateRangeMode: DateRangeMode;

  constructor(deviceId: string, dateRangeMode: DateRangeMode) {
    this.deviceId = deviceId;
    this.dateRangeMode = dateRangeMode;
  }

  public equals(other: ChartSettings) : boolean {
    return this.deviceId == other.deviceId && this.dateRangeMode == other.dateRangeMode;
  }
}
