import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService, TaskCreateRequest } from '../../services/task.service';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent {
  @Output() taskCreated = new EventEmitter<void>();
  
  taskForm: FormGroup;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(120)]],
      dueDate: ['']
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      this.loading = true;
      this.error = null;
      
      const formValue = this.taskForm.value;
      const task: TaskCreateRequest = {
        title: formValue.title.trim(),
        dueDate: formValue.dueDate ? new Date(formValue.dueDate) : undefined
      };

      this.taskService.createTask(task).subscribe({
        next: () => {
          this.loading = false;
          this.taskForm.reset();
          this.taskCreated.emit(); // Notificar al componente padre
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al crear la tarea';
          console.error('Error:', err);
        }
      });
    }
  }

  get title() { return this.taskForm.get('title'); }
  get dueDate() { return this.taskForm.get('dueDate'); }
}