import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {Router} from "@angular/router";

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './server-error.component.html',
})
export class ServerErrorComponent implements OnInit {
  error: any;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state;
  }

  ngOnInit(): void {
  }
}
