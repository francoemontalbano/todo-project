import { Component, ViewChild } from '@angular/core';
import { TaskListComponent } from './components/task-list/task-list.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Todo Angular Client';
  
  @ViewChild(TaskListComponent) taskList!: TaskListComponent;

  onTaskCreated(): void {
    // Cuando se crea una nueva tarea, actualizar la lista
    this.taskList.loadTasks();
  }
}