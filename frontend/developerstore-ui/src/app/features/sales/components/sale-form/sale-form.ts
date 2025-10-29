import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  SimpleChanges,
  ChangeDetectorRef
} from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../models/sale.model';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

registerLocaleData(localePt);

@Component({
  selector: 'app-sale-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [CurrencyPipe],
  templateUrl: './sale-form.html',
  styleUrls: ['./sale-form.css']
})
export class SaleForm implements OnInit, OnChanges {
  @Input() saleToEdit: Sale | null = null;
  @Output() saleSaved = new EventEmitter<void>();

  saleForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private salesService: SalesService,
    private cdr: ChangeDetectorRef,
    private currencyPipe: CurrencyPipe
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('saleToEdit' in changes) {
      setTimeout(() => this.applyEditIfNeeded(), 0);
    }
  }

  private initForm(): void {
    this.saleForm = this.fb.group({
      id: [0],
      saleNumber: ['', Validators.required],
      customer: ['', Validators.required],
      branch: ['', Validators.required],
      date: ['', Validators.required],
      items: this.fb.array([]),
      totalAmount: [{ value: 0, disabled: true }]
    });

    if (this.items.length === 0) {
      this.addItem();
    }

    this.items.valueChanges.subscribe(() => this.recalculateAll());
  }

  private applyEditIfNeeded(): void {
    if (!this.saleToEdit) {
      this.saleForm.patchValue({
        id: 0,
        saleNumber: '',
        customer: '',
        branch: '',
        date: new Date()
      });
      this.saleForm.get('saleNumber')?.enable();
      if (this.items.length === 0) this.addItem();
      this.cdr.markForCheck();
      return;
    }

    const sale = this.saleToEdit;
    this.saleForm.patchValue({
      id: sale.id ?? 0,
      saleNumber: sale.saleNumber ?? '',
      customer: sale.customer ?? '',
      branch: sale.branch ?? '',
      date: sale.date ?? new Date()
    });

    this.saleForm.get('saleNumber')?.disable();

    this.items.clear();

    if (sale.items && sale.items.length) {
      for (const it of sale.items) {
        const g = this.createItemGroup();
        g.patchValue({
          id: it.id,
          product: it.product,
          quantity: it.quantity,
          unitPrice: it.unitPrice,
          discount: it.discount ?? 0,
          total: it.total ?? (it.quantity * it.unitPrice)
        }, { emitEvent: false });
        this.items.push(g);
      }
    } else {
      this.addItem();
    }

    setTimeout(() => {
      this.recalculateAll();
      this.cdr.detectChanges();
    }, 0);
  }

  get items(): FormArray {
    return this.saleForm.get('items') as FormArray;
  }

  private createItemGroup(): FormGroup {
    const g = this.fb.group({
      id: [],
      product: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1), Validators.max(20)]],
      unitPrice: [0.01, [Validators.required, Validators.min(0.01)]],
      discount: [{ value: 0, disabled: true }],
      total: [{ value: 0, disabled: true }]
    });

    g.get('quantity')!.valueChanges.subscribe(() => this.updateItemTotals(g));
    g.get('unitPrice')!.valueChanges.subscribe(() => this.updateItemTotals(g));

    this.updateItemTotals(g);
    return g;
  }

  addItem(): void {
    this.items.push(this.createItemGroup());
    setTimeout(() => this.cdr.detectChanges(), 0);
  }

  removeItem(index: number): void {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    } else {
      const g = this.items.at(0);
      g.patchValue({ product: '', quantity: 1, unitPrice: 0.01 }, { emitEvent: false });
      this.updateItemTotals(g as FormGroup);
    }
    setTimeout(() => {
      this.recalculateAll();
      this.cdr.detectChanges();
    }, 0);
  }

  private updateItemTotals(g: FormGroup): void {
    const q = Number(g.get('quantity')?.value) || 0;
    const p = Number(g.get('unitPrice')?.value) || 0;

    let discount = 0;
    if (q >= 10 && q <= 20) discount = 0.2;
    else if (q >= 4) discount = 0.1;

    const total = q * p * (1 - discount);

    g.get('discount')!.setValue(discount, { emitEvent: false });
    g.get('total')!.setValue(Number(total.toFixed(2)), { emitEvent: false });

    this.recalculateAll();
  }

  recalculateAll(): void {
    const totalAmount = this.items.controls
      .map(i => i.get('total')?.value || 0)
      .reduce((a, b) => a + b, 0);

    const totalRounded = Math.round(totalAmount * 100) / 100;
    this.saleForm.get('totalAmount')?.setValue(totalRounded);
  }

  formatCurrency(value: number): string {
    if (value === null || value === undefined) return '';
    return this.currencyPipe.transform(value, 'BRL', 'symbol', '1.2-2', 'pt-BR') ?? '';
  }

  onUnitPriceInput(event: Event, index: number): void {
    const input = event.target as HTMLInputElement;
    const rawValue = input.value.replace(/[^\d]/g, '');
    const numericValue = rawValue ? parseFloat(rawValue) / 100 : 0;

    const itemGroup = this.items.at(index);
    if (!itemGroup) return;

    itemGroup.get('unitPrice')?.setValue(numericValue, { emitEvent: false });

    const quantity = itemGroup.get('quantity')?.value || 0;
    const discount = itemGroup.get('discount')?.value || 0;

    const total = numericValue * quantity * (1 - discount);
    const totalRounded = Math.round(total * 100) / 100;
    itemGroup.get('total')?.setValue(totalRounded, { emitEvent: false });

    input.value = this.formatCurrency(numericValue);

    this.recalculateAll();
  }

  updateUnitPrice(event: Event, index: number): void {
    const input = event.target as HTMLInputElement;
    const rawValue = input.value.replace(/[^\d,.-]/g, '').replace(',', '.');
    const numericValue = parseFloat(rawValue) || 0;

    const itemGroup = this.items.at(index);
    if (!itemGroup) return;

    itemGroup.get('unitPrice')?.setValue(numericValue, { emitEvent: false });

    const quantity = itemGroup.get('quantity')?.value || 0;
    const discount = itemGroup.get('discount')?.value || 0;

    const total = numericValue * quantity * (1 - discount);
    const totalRounded = Math.round(total * 100) / 100;
    itemGroup.get('total')?.setValue(totalRounded, { emitEvent: false });

    input.value = this.formatCurrency(numericValue);

    this.recalculateAll();
  }

  onSubmit(): void {
    if (this.saleForm.invalid) {
      this.saleForm.markAllAsTouched();
      return;
    }

    const payload = this.saleForm.getRawValue();

    if (payload.date) {
      const d = new Date(payload.date);
      const pad = (n: number) => n.toString().padStart(2, '0');
      payload.date = `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}:${pad(d.getSeconds())}`;
    }

    const isCreate = !payload.id || payload.id === 0;

    if (payload.items && Array.isArray(payload.items)) {
      payload.items.forEach((item: any) => {
        if (isCreate || item.id === null) {
          delete item.id;
        }
      });
    }

    if (isCreate) {
      delete payload.id;
    }

    const obs = payload.id
      ? this.salesService.update(payload)
      : this.salesService.create(payload);

    obs.subscribe({
      next: () => {
        this.saleForm.reset();
        this.items.clear();
        this.addItem();
        this.saleForm.get('saleNumber')?.enable();
        this.saleSaved.emit();
      },
      error: (err) => console.error('Error saving sale:', err)
    });
  }

  cancelEdit(): void {
    this.saleToEdit = null;
    this.saleForm.reset();
    this.items.clear();
    this.addItem();
    this.saleForm.get('saleNumber')?.enable();
    this.cdr.detectChanges();
  }
}
