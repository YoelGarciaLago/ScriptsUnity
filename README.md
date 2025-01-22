# Scripts de Player y de la Cámara   
## Antes de nada   
Para entender mejor los scripts hay que entender el porqué de estos métodos:   
**void Start()**
Es el que se inicia al iniciar el programa 
**void Update()**
Se ejecuta de manera intermitente para mostrar las acciones por pantalla
**void FixedUpdate()**
Se ejecuta de manera similar al método de Update pero en intervalos fijos
**void LateUpdate()**
Se ejecuta de manera similar a Update pero al final de todos los demas update
### Script player   
Este script es el que le indica al programa como se mueve el jugador:   
``
void Start()
{
    rb = GetComponent<Rigidbody>();
}
``
Esto indica que el rigidbody se asigne al personaje (a la bola).   
``
void OnMove(InputValue movementValue)
{
    Vector2 movementVector = movementValue.Get<Vector2>();
    movementX = movementVector.x;
    movementY = movementVector.y;
    movement = new Vector3(movementX, 0.0f, movementY);
}
``
Esto indica al programa que se mueva a través de WASD y que cada valor se mueva en un eje determinado. El vector 2 pilla los movimientos de adelante y atrás a través del InputValue y el vector 3 es el encargado de mover el personaje.   

``
private void FixedUpdate()
{
    rb.AddForce(movement * speed);
}
``
Método en donde le da la fuerza cuando se pulsa un botón de movimiento

### Cámara fija

``
void Start()
{
    offset = transform.position - player.transform.position;
}
``
Marca la posición de la cámara en el plano para que mantenga siempre la misma distancia.  
``
void LateUpdate()
{
    transform.position = player.transform.position + offset;
}
``
Hace que se muev ala cámara al mover el personaje (hay que asignarle uno al script en el ide).   

### Cámara FPS
``
void Start()
{
    offset = transform.position - player.transform.position;
    Cursor.lockState = CursorLockMode.Locked;
    playerRigidbody = player.GetComponent<Rigidbody>();
}
``
Lo mismo que el script de arriba pero se le añade un jugador (su pov) y hace que se mueva con él).   

``
void Update()
{
    if (Input.GetKeyDown(KeyCode.F))
    {
        isFirstPerson = !isFirstPerson;
        Cursor.lockState = isFirstPerson ? CursorLockMode.Locked : CursorLockMode.None;
    }

    if (isFirstPerson)
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
``
Hace que, aparte de poder cambiar la cámara con F, que cambie de cámara a la fija. Si es la FPS, pilla los movimientos de X y de Y (del ratón) y con MathF.Clamp limita la rotación (para que no le de vueltas la cabeza como una peonza) y con Euler haces que los movimientos del ratón se refejen en la cámara y haga que se mueva en la misma dirección.  

``
void FixedUpdate()
{
    if (isFirstPerson)
    {
        HandleFirstPersonMovement();
    }
}
``
Mira si está en 1ra persona para ejecutar el método.   
``
private void HandleFirstPersonMovement()
{
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Vector3 forward = transform.forward;
    Vector3 right = transform.right;

    forward.y = 0;
    right.y = 0;
    forward.Normalize();
    right.Normalize();

    Vector3 movementDirection = (forward * moveVertical + right * moveHorizontal).normalized;

    playerRigidbody.velocity = movementDirection * movementSpeed + new Vector3(0, playerRigidbody.velocity.y, 0);
}
``
Este método es el encargado de que cada vez que se mueva el personaje con la cámara girada (pongamos por ejemplo 90º a la izquierda), que use la W  para ir hacia delante. Si no se pusiera este método, al girar la cámara, si le dieses a la W es como si te movieras a la derecha.   
Con el siguiente código:   

``
Vector3 forward = transform.forward;
    Vector3 right = transform.right;

    forward.y = 0;
    right.y = 0;
    forward.Normalize();
    right.Normalize();
``

Haces que cuando le des a la W, solo te mueva hacia adelante (lo que sería la izquierda con la cámara sin mover) y que no te sumara también el movimiento de la cámara sin girar (en nuestro caso, que se moviera para la adelante y a la derecha con la cámara girada o, en su defecto, a la izquierda y hacian delante con la cámra sin girar).  
