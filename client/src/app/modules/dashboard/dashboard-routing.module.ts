import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DevicesComponent } from "./pages/devices/devices.component";
import { GuardService } from "../shared/services/guard.service";
import { PacketsComponent } from "./pages/packets/packets.component";

const routes: Routes = [
  { path: 'devices', component: DevicesComponent, canActivate: [GuardService] },
  { path: 'packets', component: PacketsComponent, canActivate: [GuardService] }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class DashboardRoutingModule {
}
