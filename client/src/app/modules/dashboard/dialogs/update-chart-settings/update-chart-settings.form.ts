import { FormControl } from "@angular/forms";
import { DateRangeMode } from "src/app/modules/graphql/services/graphql.service";

export interface UpdateChartSettingsForm {
  deviceId: FormControl<string>;
  dateRangeMode: FormControl<DateRangeMode>;
}
