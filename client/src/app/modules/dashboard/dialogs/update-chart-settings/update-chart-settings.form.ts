import { FormControl } from "@angular/forms";
import { DateRangeMode } from "../../enums/date-range-mode.enum";

export interface UpdateChartSettingsForm {
  deviceId: FormControl<string>;
  dateRangeMode: FormControl<DateRangeMode>;
}
