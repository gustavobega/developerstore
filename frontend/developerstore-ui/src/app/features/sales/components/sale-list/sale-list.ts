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
  saleSelecionado: Sale | null = null;

  constructor(
    private saleService: SalesService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.saleService.getAll().subscribe({
      next: (data) => {
        this.sales = [...data];
        this.saleSelecionado = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro ao carregar vendas:', err);
      }
    });
  }

  editarSale(sale: Sale): void {
    this.saleSelecionado = { ...sale };

    this.cdr.detectChanges();
  }

  excluirSale(id: string | undefined): void {
    if (id && confirm('Tem certeza que deseja cancelar esta venda?')) {
      this.saleService.cancel(id).subscribe({
        next: () => {
          this.loadSales();
        },
        error: (err) => console.error('Erro ao excluir venda:', err)
      });
    }
  }

  onSaleSalvo(): void {
    this.saleSelecionado = null;
    this.loadSales();
    this.cdr.detectChanges();
  }

  calcularDescontoTotal(sale: any): number {
    if (!sale.items || sale.items.length === 0) return 0;

    const descontos: number[] = sale.items.map((i: any) => i.discount || 0);

    const media = descontos.reduce((a: number, b: number) => a + b, 0) / descontos.length;

    return media * 100;
  }
}