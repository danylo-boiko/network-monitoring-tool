import { FormControl } from "@angular/forms";
import { IpFilterAction } from "../../../graphql/services/graphql.service";

export interface UpdateIpFilterForm {
  filterAction: FormControl<IpFilterAction>;
  comment: FormControl<string | null>;
}
