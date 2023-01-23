import {
  Component,
  OnInit,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { Item } from '../item-service/Item'
import { ItemService } from '../item-service/item.service'
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ItemService]
})
export class AppComponent implements OnInit {
  @ViewChild('readOnlyTemplate', { static: false }) readOnlyTemplate: TemplateRef<any> | undefined;
  @ViewChild('editTemplate', { static: false }) editTemplate: TemplateRef<any> | undefined;

  editedItem: Item | null = null;
  items: Array<Item>;
  isNewRecord: boolean = false;
  statusMessage: string = "";
  status: boolean = false;

  constructor(private serv: ItemService) {
    this.items = new Array<Item>();
  }

  ngOnInit() {
    this.loadItems();
  }

  // Public operations
  addItem() {
    this.editedItem = new Item(0, "", 0, false);
    this.items.push(this.editedItem);
    this.isNewRecord = true;
  }

  editItem(item: Item) {
    this.editedItem = new Item(item.id, item.purchase, item.price, item.done);
  }

  loadTemplate(item: Item) {
    if (this.editedItem && this.editedItem.id === item.id) {
      return this.editTemplate as TemplateRef<any> | null;
    } else {
      return this.readOnlyTemplate as TemplateRef<any> | null;
    }
  }

  saveItem() {
    if (this.editedItem!.purchase == null && this.editedItem!.price == 0) {
      this.updateStatus('Данные успешно добавлены', true);
      this.loadItems();
    }

    if (this.isNewRecord) {
      this.serv.createItem(this.editedItem as Item).subscribe(
        () => {
          this.updateStatus('Данные успешно добавлены', true);
          this.loadItems();
        },
        (error: HttpErrorResponse) => {
          console.error(error.message);
          this.updateStatus('Произошла ошибка', false);
          this.loadItems();
        });

      this.isNewRecord = false;
      this.editedItem = null;
    } else {
      this.serv.updateItem(this.editedItem as Item).subscribe(
        () => {
          this.updateStatus('Данные успешно добавлены', true);
          this.loadItems();
        },
        (error: HttpErrorResponse) => {
          console.error(error.message);
          this.updateStatus('Произошла ошибка', false);
          this.loadItems();
        });

      this.editedItem = null;
    }
  }

  saveCurrentItem(item: Item) {
    this.serv.updateItem(item).subscribe(
      () => {
        this.updateStatus('Данные успешно добавлены', true);
        this.loadItems();
      },
      (error: HttpErrorResponse) => {
        console.error(error.message);
        this.updateStatus('Произошла ошибка', false);
        this.loadItems();
      });
  }

  cancel() {
    if (this.isNewRecord) {
      this.items.pop();
      this.isNewRecord = false;
    }
    this.editedItem = null;
  }

  deleteItem(item: Item) {
    if (confirm(`Are you sure to delete ${item.purchase}(${item.price}) ?`)) {
      this.serv.deleteItem(item.id).subscribe(
        () => {
          this.updateStatus(`${item.purchase} (${item.price}) успешно удалён`, true);
          this.loadItems();
        },
        (error: HttpErrorResponse) => {
          console.error(error.message);
          this.updateStatus(`Произошла ошибка при удалении ${item.purchase} (${item.price})`, false);
          this.loadItems();
        });
    }
  }

  // Private operations
  private loadItems() {
    try {
      this.serv.getItems().subscribe((data: Array<Item>) => {
        this.items = data; //.sort(a => a.done ? 1 : 0);
        console.info(`Got items from server: ${JSON.stringify(this.items)}`);
      });
    }
    catch {
      console.error('Failed to connect to server');
    }
  }

  private updateStatus(newStatusMessage: string, newStatus: boolean) {
    this.statusMessage = newStatusMessage;
    this.status = newStatus;
    setTimeout(() => {
      this.clearStatus();
    }, 5000);
  }

  private clearStatus() {
    this.statusMessage = "";
    this.status = false;
  }
}