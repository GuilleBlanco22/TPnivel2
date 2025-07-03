//const Phaser = require("phaser");

var config = {
    type: Phaser.AUTO,
    width: 800,
    height: 600,
    physics: {
        default: 'arcade',
        arcade: {
            gravity: { y: 300 }, //gravedad
            debug: false
        }
    },


    scene: {
        preload: preload,
        create: create,
        update: update
    }
};

var score = 0;
var scoreText;
var gameOver = false;

var game = new Phaser.Game(config);

function preload() {
    this.load.image('sky', 'assets/sky.png');
    this.load.image('ground', 'assets/platform.png');
    this.load.image('esfera2', 'assets/esfera2.png');
    this.load.image('nave', 'assets/nave.png');
    this.load.spritesheet('goku1', 'assets/goku1.png', { frameWidth: 77, frameHeight: 110 });
        

}
function create() {
    this.add.image(400, 300, 'sky');

    // Plataformas
    var platforms = this.physics.add.staticGroup();

    platforms.create(400, 568, 'ground').setScale(2).refreshBody();

    platforms.create(600, 400, 'ground');
    platforms.create(50, 250, 'ground');
    platforms.create(750, 220, 'ground');

    // Jugador
    player = this.physics.add.sprite(50, 450, 'goku1');//donde se va a crear el personaje
    player.setScale(0.9); // cambiar el tamaño del personaje
    player.setCollideWorldBounds(true); //no salir de la pantalla
    player.setBounce(0.3); //rebote al iniciar

    // Animaciones del jugador
    this.anims.create({
        key: 'left',
        frames: this.anims.generateFrameNumbers('goku1', { start: 0, end: 3 }),
        frameRate: 10,
        repeat: -1
    });

    this.anims.create({
        key: 'turn',
        frames: [{ key: 'goku1', frame: 0 }],//posicion inicial del personaje
        frameRate: 20
    });

    this.anims.create({
        key: 'right',
        frames: this.anims.generateFrameNumbers('goku1', { start: 0, end: 2 }),
        frameRate: 10,
        repeat: -1
    });
    
    // Añadir colisiones entre el jugador y las plataformas
    this.physics.add.collider(player, platforms);
    // Entrada de teclado
    this.cursors = this.input.keyboard.createCursorKeys();

    esferas = this.physics.add.group({
        key: 'esfera2',
        repeat: 11,
        setXY: { x: 12, y: 0, stepX: 70 }//posicion inicial de las esferas
        
    });

    esferas.children.iterate(function (esfera) {
        // Asignar una velocidad aleatoria a cada esfera
        esfera.setBounce(Phaser.Math.FloatBetween(0.4, 0.8));
        //esfera.setVelocity(Phaser.Math.Between(-200, 200), 20);
       // esfera.setCollideWorldBounds(true);
       // esfera.setGravityY(300); // Añadir gravedad a las esferas
       esfera.setScale(0.3); // Cambiar el tamaño de las esferas
    if (esfera.body) {
        esfera.body.setSize(esfera.displayWidth, esfera.displayHeight); // ← Ajusta el área de colisión
    }
    });
    this.physics.add.collider(esferas, platforms);
    this.physics.add.overlap(player, esferas, collectEsfera, null, this);
    // Añadir colisión entre el jugador y las plataformas
    
    scoreText = this.add.text(16, 16, 'Score: 0', { fontSize: '32px', fill: '#fff' });

    naves = this.physics.add.group();
    this.physics.add.collider(naves, platforms);

    this.physics.add.collider(player, naves, hitNave, null, this);//termina juego cuando el jugador toca la nave
      



    // Asignar el jugador a la variable this.player
    this.player = player;
    // Añadir el jugador a las plataformas
    platforms.add(player);
    // Ajustar el tamaño del jugador
    player.setSize(70, 48); // Ajusta el tamaño del jugador según el sprite
    player.setOffset(0, 60); // Ajusta el offset del jugador para que se alinee correctamente con las plataformas
    // Añadir el jugador a la escena
    this.add.existing(player);
    // Añadir el jugador a las plataformas
    platforms.add(player);



    
}
function update() {

    if (gameOver) {
        return; // Si el juego ha terminado, no actualizamos nada más
    }


    if (this.cursors.left.isDown) {
        this.player.setVelocityX(-160);
        this.player.anims.play('left', true);
    } else if (this.cursors.right.isDown) {
        this.player.setVelocityX(160);
        this.player.anims.play('right', true);
    } else {
        this.player.setVelocityX(0);
        this.player.anims.play('turn');
    }

    if (this.cursors.up.isDown && this.player.body.touching.down) {
        this.player.setVelocityY(-330);
    }
    
}
function collectEsfera(player, esfera) {
    esfera.disableBody(true, true); // Deshabilitar la esfera recogida
    score += 10; // Incrementar la puntuación
    scoreText.setText('Score: ' + score); // Actualizar el texto de la puntuación
    
    if( esferas.countActive(true) === 0) {
        // Si no quedan esferas activas, reiniciar el grupo de esferas
        esferas.children.iterate(function (esfera) {
            esfera.enableBody(true, esfera.x, 0, true, true);
            //esfera.setVelocity(Phaser.Math.Between(-200, 200), 20); // Asignar una velocidad aleatoria a las nuevas esferas
        });
        var x = (player.x < 400) ? Phaser.Math.Between(400, 800) : Phaser.Math.Between(0, 400);
        
        var nave = naves.create(x, 16, 'nave');
        nave.setScale(0.1); // tamaño de la nave
        nave.body.setSize(nave.displayWidth, nave.displayHeight);//ajusta tamaño fisico


        nave.setBounce(1); // Hacer que la nave rebote
        nave.setCollideWorldBounds(true); // Hacer que la nave no salga de los límites del mundo
        nave.setVelocity(Phaser.Math.Between(-200, 200), 20); // Asignar una velocidad aleatoria a la nave
    }

}

function hitNave(player, nave) {
    this.physics.pause(); // Pausar la física del juego
    player.setTint(0xff0000); // Cambiar el color del jugador a rojo
    player.anims.play('turn'); // Reproducir la animación de giro

    gameOver = true; // Marcar el juego como terminado
}

