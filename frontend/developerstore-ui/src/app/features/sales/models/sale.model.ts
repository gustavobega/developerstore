export interface SaleItem {
  id: string;
  product: string;
  quantity: number;
  unitPrice: number;
  discount?: number;
  total?: number;
}

export interface Sale {
  id: string;
  saleNumber: number;
  date: Date;
  customer: string;
  branch: string;
  items: SaleItem[];
  totalAmount?: number;
  isCancelled?: boolean;
}