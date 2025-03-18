# Documentacion

# Estructura del Proyecto

El proyecto está compuesto por los siguientes archivos principales:

**ApiCorePayment**: Contiene un minimal Api simulando el api de payment donde por medio de un endpoint se expone el envió de mensajes a las colas que estén escuchando, de forma dinámica el mismo.

**orders.PaymentWorker**: Incluye la lógica de un consumidor que procesa el estado de la orden y actúa en consecuencia, en este caso simulando un segundo mensaje con delay hacia el segundo consumidor en este caso orders.TicketingWorker

**orders.TicketingWorker**: Incluye la lógica de un consumidor que procesa el mensaje recibido bien sea por el api de payment o el Worker de **orders.PaymentWorker** 

## Flujo de Trabajo

1. Enviar una solicitud: Enviar una solicitud a la ruta establecida por el api

- [https://localhost:44346/publish/](https://localhost:44346/publish/declined){orderStatus}

donde **orderStatus** puede ser cualquier estado que se requiera, esto hará que **orders.PaymentWorker** reciba el mensaje
        
una segunda opcion seria enviar 
- [https://localhost:44346/publish/](https://localhost:44346/publish/declined)approved
donde publicara un mensaje y sera leido por dos procesos **orders.PaymentWorker** y **orders.TicketingWorker**
    
2. **Procesamiento por PaymentWorker**: El trabajador de pagos recibe el mensaje con cualquier tipo de estado, y quemara un mensaje con el estado aprobado donde se enviara con un delay de un minuto hacia el siguiente worker.
3. **Procesamiento por TicketingWorker**: El trabajador de ticketing recibira solo mensajes con el estado aprobado y quemara mensajes por consola recibiendo dicho mensaje.

## Requisitos Previos

a. Se asume conocimiento e instalacion previa del sdk de .net 8 asi como de un entorno para ejecucion local

- [https://dotnet.microsoft.com/es-es/download/dotnet/8.0](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
- [https://visualstudio.microsoft.com/es/](https://visualstudio.microsoft.com/es/)
- [https://code.visualstudio.com](https://code.visualstudio.com/)

b. instalacion del lenguaje de desarrollo erlang
ingresar al link en mención e instalar el archivo segun corresponde al entorno, seguir los pasos descritos en la documentacion.
[https://www.erlang.org/downloads](https://www.erlang.org/downloads)


c. instalacion del servio de rabbitMq de manera local

[https://www.rabbitmq.com/docs/download](https://www.rabbitmq.com/docs/download)
    
 
 ejecutar servidor
 

```powershell
rabbitmq-server
```

para obtener la interfaz web que nos permite visualizar de una mejor forma el servicio de rabbitmq podemos habilitar el plugin desde la terminal de comandos

```powershell

rabbitmq-plugins enable rabbitmq_management
```

usuarios por defecto para ingresar

guest → usuario

guest → clave

lo siguiente que debemos hacer es instalar y habilitar el plugin de delayed queue

[https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/blob/main/README.md](https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/blob/main/README.md)

aqui los binarios del plugin

[https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases](https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases)

descargar el .ez

ruta por defecto donde se almacenan los plugins

C:\Program Files\RabbitMQ Server\rabbitmq_server-4.0.7\plugins

se debe apagar y encender el servicio para refrescar los archivos desde la terminal de comandos

```powershell
rabbitmq-service stop
rabbitmq-service start
```

luego se habilita el plugin

```powershell
rabbitmq-plugins enable rabbitmq_delayed_message_exchange
```

verificar plugins instalados

```powershell
rabbitmq-plugins list
```

NOTA: si ya existe un exchange creado parece que rabbitmq no puede sobre escribirlo y opta por fallar, por lo que para que tome el nuevo tipo de exchange se debe eliminar el ya existente o cambiarle el nombre

si tenemos todo de manera correcta podremos acceder a la interfaz web y veremos en caso de haber ejecutado el proyecto observar la siguiente imagen

![image](https://github.com/user-attachments/assets/3376f673-9bd9-4ad5-89b9-f23eb373de53)


## levantar todos los proyectos al tiempo

si estamos en un entorno como visual studio 2022, en el apartado de configurar proyectos de inicio, realizaremos esta configuracion

![image 1](https://github.com/user-attachments/assets/75f55126-f272-4a18-ae2c-c16f8fee5e12)


en caso de estar en visual studio code  ejecutaremos los siguientes comandos en terminales separadas

```powershell
dotnet run --project ApiCorePayment/ApiCorePayment.csproj
dotnet run --project orders.PaymentWorker/orders.PaymentWorker.csproj
dotnet run --project orders.TicketingWorker/orders.TicketingWorker.csproj
```
