import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './vessel-navigation-component/app.component';
import { FormsModule }   from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'
import { IgxRadialGaugeModule } from 'igniteui-angular-gauges';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    IgxRadialGaugeModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }