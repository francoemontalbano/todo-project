import { Component, OnInit } from '@angular/core';
import { TaskService, TaskResponse, TaskQueryParams } from '../../services/task.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: TaskResponse[] = [];
  loading = false;
  error: string | null = null;
  
  // Paginación
  currentPage = 1;
  pageSize = 10;
  totalTasks = 0;
  
  // Filtros
  statusFilter: 'all' | 'pending' | 'done' = 'all';
  sortBy: 'createdAt' | 'dueDate' = 'createdAt';
  sortOrder: 'asc' | 'desc' = 'desc';

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading = true;
    this.error = null;
    
    const params: TaskQueryParams = {
      page: this.currentPage,
      pageSize: this.pageSize,
      sort: this.sortBy,
      order: this.sortOrder
    };
    
    if (this.statusFilter !== 'all') {
      params.status = this.statusFilter;
    }

    this.taskService.getTasks(params).subscribe({
      next: (response) => {
        this.tasks = response.items;
        this.totalTasks = response.total;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar las tareas';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onStatusFilterChange(status: 'all' | 'pending' | 'done'): void {
    this.statusFilter = status;
    this.currentPage = 1;
    this.loadTasks();
  }

  onSortChange(sort: 'createdAt' | 'dueDate', order: 'asc' | 'desc'): void {
    this.sortBy = sort;
    this.sortOrder = order;
    this.currentPage = 1;
    this.loadTasks();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadTasks();
  }

  completeTask(taskId: number): void {
    this.taskService.completeTask(taskId).subscribe({
      next: () => {
        this.loadTasks(); // Recargar la lista
      },
      error: (err) => {
        this.error = 'Error al completar la tarea';
        console.error('Error:', err);
      }
    });
  }

  deleteTask(taskId: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar esta tarea?')) {
      this.taskService.deleteTask(taskId).subscribe({
        next: () => {
          this.loadTasks(); // Recargar la lista
        },
        error: (err) => {
          this.error = 'Error al eliminar la tarea';
          console.error('Error:', err);
        }
      });
    }
  }

  get totalPages(): number {
    return Math.ceil(this.totalTasks / this.pageSize);
  }
}