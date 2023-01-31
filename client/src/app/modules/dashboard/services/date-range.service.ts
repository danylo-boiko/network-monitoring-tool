import { Injectable } from '@angular/core';
import { DateRangeMode } from "../enums/date-range-mode.enum";
import { DateRange } from '../models/date-range.model';

@Injectable({
  providedIn: 'root'
})
export class DateRangeService {
  public getDateRange(rangeMode: DateRangeMode): DateRange {
    switch (rangeMode) {
      case DateRangeMode.Day:
        return this.getDayDateRange();
      case DateRangeMode.Week:
        return this.getWeekDateRange();
      case DateRangeMode.Month:
        return this.getMonthDateRange();
      default:
        throw "Unimplemented date range mode";
    }
  }

  private getDayDateRange(): DateRange {
    return new DateRange(new Date(), new Date());
  }

  private getWeekDateRange(): DateRange {
    return new DateRange(new Date(), new Date());
  }

  private getMonthDateRange(): DateRange {
    return new DateRange(new Date(), new Date());
  }
}
