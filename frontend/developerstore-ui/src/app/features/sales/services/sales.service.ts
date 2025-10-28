import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sale } from '../models/sale.model';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  private apiUrl = 'http://127.0.0.1:5132/api/sales'; 

  constructor(private http: HttpClient) {}

  getAll(): Observable<Sale[]> {
    return this.http.get<Sale[]>(this.apiUrl);
  }

  getById(id: string): Observable<Sale> {
    return this.http.get<Sale>(`${this.apiUrl}/${id}`);
  }

  create(sale: Sale): Observable<Sale> {
    return this.http.post<Sale>(this.apiUrl, sale);
  }

  update(sale: Sale): Observable<Sale> {
    return this.http.put<Sale>(`${this.apiUrl}/${sale.id}`, sale);
  }

  cancel(id: string): Observable<Sale> {
    return this.http.delete<Sale>(`${this.apiUrl}/${id}`);
  }
}
