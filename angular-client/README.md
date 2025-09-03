# Todo Angular Client

Este es el cliente Angular que se conecta con la API de .NET Todo.

## Configuración

### Prerrequisitos
- Node.js (versión 18 o superior)
- Angular CLI (`npm install -g @angular/cli`)

### Instalación
1. Navega al directorio del cliente Angular:
   ```bash
   cd angular-client
   ```

2. Instala las dependencias:
   ```bash
   npm install
   ```

### Ejecución

1. **Ejecutar la API de .NET:**
   ```bash
   cd ../Todo.Api
   dotnet run
   ```
   La API estará disponible en `http://localhost:5000`

2. **Ejecutar el cliente Angular:**
   ```bash
   cd ../angular-client
   ng serve
   ```
   La aplicación estará disponible en `http://localhost:4200`

## Funcionalidades

- ✅ Crear nuevas tareas
- ✅ Listar tareas con filtros (estado, fecha límite)
- ✅ Marcar tareas como completadas
- ✅ Eliminar tareas
- ✅ Paginación
- ✅ Ordenamiento por fecha de creación o fecha límite

## Estructura del Proyecto

```
angular-client/
├── src/
│   ├── app/
│   │   ├── components/
│   │   │   ├── task-list/       # Componente lista de tareas
│   │   │   └── task-form/       # Componente formulario de tareas
│   │   ├── services/
│   │   │   └── task.service.ts  # Servicio para comunicarse con la API
│   │   ├── app.component.*      # Componente principal
│   │   └── app.module.ts        # Módulo principal
│   ├── index.html
│   ├── main.ts
│   └── styles.css
├── package.json
└── angular.json
```

## API Endpoints

La aplicación se conecta a los siguientes endpoints de tu API .NET:

- `GET /tasks` - Obtener lista de tareas
- `POST /tasks` - Crear nueva tarea
- `GET /tasks/{id}` - Obtener tarea específica
- `PUT /tasks/{id}` - Actualizar tarea
- `PATCH /tasks/{id}/complete` - Marcar como completada
- `DELETE /tasks/{id}` - Eliminar tarea

## Configuración de CORS

La API ya está configurada para permitir conexiones desde `http://localhost:4200`.
Si necesitas cambiar el puerto o dominio, modifica la configuración CORS en `Todo.Api/Program.cs`.