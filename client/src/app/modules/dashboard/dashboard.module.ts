import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from "./dashboard-routing.module";
import { MatListModule } from "@angular/material/list";
import { MatButtonModule } from "@angular/material/button";
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { MatCardModule } from "@angular/material/card";
import { NgApexchartsModule } from "ng-apexcharts";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatTableModule } from "@angular/material/table";
import { MatIconModule } from "@angular/material/icon";
import { MatPaginatorModule } from "@angular/material/paginator";
import { NavbarComponent } from "./components/navbar/navbar.component";
import { MatToolbarModule } from "@angular/material/toolbar";
import { DeleteIpFilterComponent } from './dialogs/delete-ip-filter/delete-ip-filter.component';
import { CreateIpFilterComponent } from './dialogs/create-ip-filter/create-ip-filter.component';
import { UpdateIpFilterComponent } from './dialogs/update-ip-filter/update-ip-filter.component';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatDialogModule } from "@angular/material/dialog";
import { MatSelectModule } from "@angular/material/select";
import { IpFiltersTableComponent } from './components/ip-filters-table/ip-filters-table.component';
import { PacketsChartComponent } from './components/packets-chart/packets-chart.component';

@NgModule({
  declarations: [
    NavbarComponent,
    DashboardComponent,
    DeleteIpFilterComponent,
    CreateIpFilterComponent,
    UpdateIpFilterComponent,
    IpFiltersTableComponent,
    PacketsChartComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MatListModule,
    MatButtonModule,
    MatCardModule,
    BrowserModule,
    MatDialogModule,
    FormsModule,
    ReactiveFormsModule,
    NgApexchartsModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
  ]
})
export class DashboardModule {
}
