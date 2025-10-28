import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../models/sale.model';
import { SaleForm } from '../sale-form/sale-form';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    SaleForm
  ],
  templateUrl: './sale-list.html',
  styleUrls: ['./sale-list.css']
})
export class SaleList implements OnInit {
  sales: Sale[] = [];
  selectedSale: Sale | null = null;

  constructor(
    private salesService: SalesService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.salesService.getAll().subscribe({
      next: (data) => {
        this.sales = [...data];
        this.selectedSale = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading sales:', err);
      }
    });
  }

  editSale(sale: Sale): void {
    this.selectedSale = { ...sale };
    this.cdr.detectChanges();
  }

  deleteSale(id: string | undefined): void {
    if (id && confirm('Are you sure you want to cancel this sale?')) {
      this.salesService.cancel(id).subscribe({
        next: () => {
          this.loadSales();
        },
        error: (err) => console.error('Error deleting sale:', err)
      });
    }
  }

  onSaleSaved(): void {
    this.selectedSale = null;
    this.loadSales();
    this.cdr.detectChanges();
  }

  calculateTotalDiscount(sale: any): number {
    if (!sale.items || sale.items.length === 0) return 0;

    const discounts: number[] = sale.items.map((i: any) => i.discount || 0);

    const average = discounts.reduce((a: number, b: number) => a + b, 0) / discounts.length;

    return average * 100;
  }
}
