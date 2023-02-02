import { FormControl } from "@angular/forms";
import { IpFilterAction } from "../../../graphql/services/graphql.service";

export interface CreateIpFilterForm {
  ip: FormControl<string>;
  filterAction: FormControl<IpFilterAction>;
  comment: FormControl<string | null>;
}
