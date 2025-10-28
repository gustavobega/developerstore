import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: 'sales', 
    loadComponent: () => 
      import('./features/sales/components/sale-list/sale-list')
      .then(m => m.SaleList)
  },
  
  { path: '', redirectTo: 'sales', pathMatch: 'full' },
];