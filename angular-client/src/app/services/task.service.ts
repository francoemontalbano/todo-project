import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

// Interfaces que coinciden con los DTOs de tu API
export interface TaskCreateRequest {
  title: string;
  dueDate?: Date;
}

export interface TaskUpdateRequest {
  title: string;
  dueDate?: Date;
}

export interface TaskResponse {
  id: number;
  title: string;
  isDone: boolean;
  dueDate?: Date;
  createdAt: Date;
}

export interface TaskListResponse {
  items: TaskResponse[];
  total: number;
  page: number;
  pageSize: number;
}

export interface TaskQueryParams {
  status?: 'pending' | 'done';
  dueBefore?: Date;
  page?: number;
  pageSize?: number;
  sort?: 'createdAt' | 'dueDate';
  order?: 'asc' | 'desc';
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = 'http://localhost:5000/tasks'; // Ajusta el puerto según tu configuración

  constructor(private http: HttpClient) { }

  // Crear nueva tarea
  createTask(task: TaskCreateRequest): Observable<TaskResponse> {
    return this.http.post<TaskResponse>(this.apiUrl, task);
  }

  // Obtener tarea por ID
  getTask(id: number): Observable<TaskResponse> {
    return this.http.get<TaskResponse>(`${this.apiUrl}/${id}`);
  }

  // Listar tareas con filtros opcionales
  getTasks(params?: TaskQueryParams): Observable<TaskListResponse> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.status) httpParams = httpParams.set('status', params.status);
      if (params.dueBefore) httpParams = httpParams.set('dueBefore', params.dueBefore.toISOString());
      if (params.page) httpParams = httpParams.set('page', params.page.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.sort) httpParams = httpParams.set('sort', params.sort);
      if (params.order) httpParams = httpParams.set('order', params.order);
    }

    return this.http.get<TaskListResponse>(this.apiUrl, { params: httpParams });
  }

  // Actualizar tarea
  updateTask(id: number, task: TaskUpdateRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, task);
  }

  // Marcar tarea como completada
  completeTask(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/complete`, {});
  }

  // Eliminar tarea
  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}