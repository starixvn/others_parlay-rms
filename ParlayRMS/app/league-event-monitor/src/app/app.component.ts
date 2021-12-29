import { Component, ElementRef } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'league-event-monitor';
  netcoreApiUrl: string;
  constructor(
    private readonly elementRef: ElementRef<HTMLElement>
  ) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url')??'';
  }
}
