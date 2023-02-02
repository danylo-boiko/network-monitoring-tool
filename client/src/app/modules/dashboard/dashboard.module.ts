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
import { MatDialogModule } from "@angular/material/dialog";
import { CreateIpFilterComponent } from './dialogs/create-ip-filter/create-ip-filter.component';
import { UpdateIpFilterComponent } from './dialogs/update-ip-filter/update-ip-filter.component';

@NgModule({
  declarations: [
    NavbarComponent,
    DashboardComponent,
    DeleteIpFilterComponent,
    CreateIpFilterComponent,
    UpdateIpFilterComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MatListModule,
    MatButtonModule,
    MatCardModule,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    NgApexchartsModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatToolbarModule,
    MatDialogModule,
  ]
})
export class DashboardModule {
}
