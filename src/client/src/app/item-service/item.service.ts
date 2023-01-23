import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Item } from './Item';

@Injectable()
export class ItemService {
  private baseUrl = "https://localhost:5001/";
  constructor(private http: HttpClient) { }

  getItems() {
    return this.http.get<Array<Item>>(`${this.baseUrl}Read`);
  }

  createItem(item: Item) {
    return this.http.get(`${this.baseUrl}Write/${item.purchase}/${item.done}/${item.price}`, { responseType:'text', observe: 'response' });
  }

  updateItem(item: Item) {
    return this.http.get(`${this.baseUrl}Edit/${item.id}/${item.purchase}/${item.done}/${item.price}`);
  }

  deleteItem(id: number) {
    return this.http.get(`${this.baseUrl}Delete/${id}`);
  }
}