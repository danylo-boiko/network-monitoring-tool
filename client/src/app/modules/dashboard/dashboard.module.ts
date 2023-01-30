import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from "@angular/router";
import { DevicesComponent } from './pages/devices/devices.component';
import { PacketsComponent } from './pages/packets/packets.component';
import { SharedModule } from "../shared/shared.module";
import { GuardService } from "../shared/services/guard.service";

const routes: Routes = [
  { path: 'devices', component: DevicesComponent, canActivate: [GuardService] },
  { path: 'packets', component: PacketsComponent, canActivate: [GuardService] }
];

@NgModule({
  declarations: [
    DevicesComponent,
    PacketsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class DashboardModule {
}
