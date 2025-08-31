import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ProjectsService } from '../../services/projects.service';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <section>
      <h2>Projeler</h2>

      <div *ngIf="loading">Yükleniyor...</div>
      <div *ngIf="error" style="color:#b00020">{{ error }}</div>

      <ul>
        <li *ngFor="let p of projects">
          <ng-container *ngIf="editingProjectId !== p.id; else editTpl">
            <a [routerLink]="['/projects', p.id]">{{ p.name }}</a>
            <span *ngIf="p.description"> - {{ p.description }}</span>
            <button (click)="onEditStart(p)" style="margin-left:8px">Düzenle</button>
            <button (click)="onDelete(p)" style="margin-left:4px">Sil</button>
          </ng-container>
          <ng-template #editTpl>
            <form [formGroup]="editForm" (ngSubmit)="onEditSave(p)">
              <input type="text" formControlName="name" placeholder="Ad" />
              <input type="text" formControlName="description" placeholder="Açıklama" style="margin-left:6px" />
              <button type="submit" [disabled]="editForm.invalid || submitting" style="margin-left:6px">Kaydet</button>
              <button type="button" (click)="onEditCancel()" style="margin-left:4px">Vazgeç</button>
            </form>
            <div *ngIf="editForm.controls.name.touched && editForm.controls.name.invalid" style="color:#b00020;">
              Ad zorunlu ve en az 3 karakter olmalı.
            </div>
          </ng-template>
        </li>
      </ul>

      <hr />
      <h3>Yeni Proje Ekle</h3>
      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <div>
          <label for="name">Ad</label><br />
          <input id="name" type="text" formControlName="name" />
          <div *ngIf="form.controls.name.touched && form.controls.name.invalid" style="color:#b00020">
            Ad zorunlu ve en az 3 karakter olmalı.
          </div>
        </div>
        <div style="margin-top:8px;">
          <label for="desc">Açıklama</label><br />
          <input id="desc" type="text" formControlName="description" />
        </div>
        <button type="submit" [disabled]="form.invalid || submitting" style="margin-top:12px;">
          {{ submitting ? 'Kaydediliyor...' : 'Ekle' }}
        </button>
      </form>
    </section>
  `
})
export class ProjectListComponent implements OnInit {
  projects: Project[] = [];
  loading = false;
  error: string | null = null;
  submitting = false;
  editingProjectId: number | null = null;

  form = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3)] }),
    description: new FormControl<string | null>('')
  });

  editForm = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3)] }),
    description: new FormControl<string | null>('')
  });

  constructor(private readonly projectsService: ProjectsService) {}

  ngOnInit(): void {
    this.fetchProjects();
  }

  fetchProjects(): void {
    this.loading = true;
    this.error = null;
    this.projectsService.getAll().subscribe({
      next: (res) => {
        this.projects = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'Projeler yüklenemedi.';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    const payload = {
      name: this.form.controls.name.value,
      description: this.form.controls.description.value ?? null
    };
    this.projectsService.create(payload).subscribe({
      next: () => {
        this.submitting = false;
        this.form.reset();
        this.fetchProjects();
      },
      error: () => {
        this.submitting = false;
        this.error = 'Proje eklenemedi.';
      }
    });
  }

  onEditStart(p: Project): void {
    this.editingProjectId = p.id;
    this.editForm.reset({ name: p.name, description: p.description ?? '' });
  }

  onEditCancel(): void {
    this.editingProjectId = null;
    this.editForm.reset();
  }

  onEditSave(p: Project): void {
    if (this.editForm.invalid) return;
    this.submitting = true;
    const payload = {
      name: this.editForm.controls.name.value,
      description: this.editForm.controls.description.value ?? null
    };
    this.projectsService.update(p.id, payload).subscribe({
      next: () => {
        this.submitting = false;
        this.editingProjectId = null;
        this.fetchProjects();
      },
      error: () => {
        this.submitting = false;
        this.error = 'Proje güncellenemedi.';
      }
    });
  }

  onDelete(p: Project): void {
    if (!confirm('Silmek istediğinize emin misiniz?')) return;
    this.projectsService.delete(p.id).subscribe({
      next: () => {
        this.fetchProjects();
      },
      error: () => {
        this.error = 'Proje silinemedi.';
      }
    });
  }
}


