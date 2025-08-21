import { Routes } from '@angular/router';
import { EmpleadosComponent } from './components/empleados/empleados';

export const routes: Routes = [
  { path: '', redirectTo: '/empleados', pathMatch: 'full' },
  { path: 'empleados', component: EmpleadosComponent },
  { path: '**', redirectTo: '/empleados' }
];
