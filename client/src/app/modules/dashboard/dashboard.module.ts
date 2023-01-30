import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { DevicesComponent } from './pages/devices/devices.component';
import { PacketsComponent } from './pages/packets/packets.component';
import { SharedModule } from "../shared/shared.module";
import { DashboardRoutingModule } from "./dashboard-routing.module";

@NgModule({
  declarations: [
    DevicesComponent,
    PacketsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    DashboardRoutingModule
  ],
  exports: [
    RouterModule
  ]
})
export class DashboardModule {
}
