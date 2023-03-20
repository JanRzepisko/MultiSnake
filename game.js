const SNAKE_SIZE = 20;
const FOOD_SIZE = 14;

const BOARD = document.getElementById("game");
const { height: B_HEIGHT, width: B_WIDTH } = BOARD.getBoundingClientRect();

class DIR {
  static UP = "up";
  static DOWN = "down";
  static LEFT = "left";
  static RIGHT = "right";
}

class Snake {
  constructor(
    color,
    isFirstPlayer,
    isLocalPlayer,
    RemotePlayerBody,
    connection,
    GameId
  ) {
    this.color = color;
    this.isFirstPlayer = isFirstPlayer;
    this.connection = connection;
    this.GameId = GameId;
    this.body = isFirstPlayer
      ? [
          { x: 140, y: 300, head: true },
          { x: 120, y: 300, head: false },
          { x: 100, y: 300, head: false },
          { x: 80, y: 300, head: false },
          { x: 60, y: 300, head: false },
          { x: 40, y: 300, head: false },
          { x: 20, y: 300, head: false },
        ]
      : [
          { x: 480, y: 300, head: true },
          { x: 500, y: 300, head: false },
          { x: 520, y: 300, head: false },
          { x: 540, y: 300, head: false },
          { x: 560, y: 300, head: false },
          { x: 580, y: 300, head: false },
        ];
    this.direction = isFirstPlayer ? DIR.RIGHT : DIR.LEFT;
    this.isLocalPlayer = isLocalPlayer;
    this.RemotePlayerBody = RemotePlayerBody;

    document.addEventListener("keydown", (e) => {
      this.changeDirection(e);
    });
  }

  updateRemotePlayerBody(RemotePlayerBody) {
    this.RemotePlayerBody = RemotePlayerBody;
  }

  changeDirection(e) {
    switch (e.key) {
      case "ArrowUp":
        if (this.direction !== DIR.DOWN) {
          this.direction = DIR.UP;
        }
        break;
      case "ArrowDown":
        if (this.direction !== DIR.UP) {
          this.direction = DIR.DOWN;
        }
        break;
      case "ArrowLeft":
        if (this.direction !== DIR.RIGHT) {
          this.direction = DIR.LEFT;
        }
        break;
      case "ArrowRight":
        if (this.direction !== DIR.LEFT) {
          this.direction = DIR.RIGHT;
        }
        break;
    }
  }

  draw() {
    // remove old elements
    const CLASS = "local";

    console.log("IN_SIDE", this.RemotePlayerBody);

    const oldElements = document.querySelectorAll("." + CLASS);

    oldElements.forEach((el) => {
      el.remove();
    });

    this.body.forEach((part) => {
      const snake = document.createElement("div");
      snake.classList.add(CLASS);
      snake.style.backgroundColor = this.color;
      snake.style.position = "absolute";
      snake.style.height = `${SNAKE_SIZE}px`;
      snake.style.width = `${SNAKE_SIZE}px`;
      snake.style.top = `${part.y}px`;
      snake.style.left = `${part.x}px`;
      snake.style.zIndex = 10;
      BOARD.appendChild(snake);
    });
  }

  move() {
    const head = { ...this.body[0] };

    // check if snake is out of board
    if (this.checkCollision(head)) {
      return;
    }

    switch (this.direction) {
      case DIR.UP:
        head.y -= SNAKE_SIZE;
        break;
      case DIR.DOWN:
        head.y += SNAKE_SIZE;
        break;
      case DIR.LEFT:
        head.x -= SNAKE_SIZE;
        break;
      case DIR.RIGHT:
        head.x += SNAKE_SIZE;
        break;
    }

    this.connection.invoke("Move", this.GameId, this.isFirstPlayer ? 0 : 1, {
      x: head.x,
      y: head.y,
    });

    this.body.unshift(head);
    this.body.pop();
  }

  checkCollision(head) {
    // move snake if it collide with wall

    // LEFT WALL
    if (head.x <= 0 && this.direction === DIR.LEFT) {
      head.x = B_WIDTH;
      return false;
    }

    // RIGHT WALL
    if (head.x >= B_WIDTH - SNAKE_SIZE && this.direction === DIR.RIGHT) {
      head.x = -SNAKE_SIZE;
      return false;
    }

    // TOP WALL
    if (head.y <= 0 && this.direction === DIR.UP) {
      head.y = B_HEIGHT;
      return false;
    }

    // BOTTOM WALL
    if (head.y >= B_HEIGHT - SNAKE_SIZE && this.direction === DIR.DOWN) {
      head.y = -SNAKE_SIZE;
      return false;
    }

    // check if snake collide with itself but not head with head
    for (let i = 1; i < this.body.length; i++) {
      if (head.x === this.body[i].x && head.y === this.body[i].y) {
        return true;
      }
    }

    if (!this.RemotePlayerBody) {
      return false;
    }

    for (let i = 0; i < this.RemotePlayerBody.length; i++) {
      if (
        head.x === this.RemotePlayerBody[i].x &&
        head.y === this.RemotePlayerBody[i].y
      ) {
        if (head.head === true && this.RemotePlayerBody[i].head === true) {
          if (this.body.length > this.RemotePlayerBody.length) {
            console.log("You win");
          } else if (this.body.length < this.RemotePlayerBody.length) {
            console.log("You lose");
          } else {
            console.log("Draw");
          }
        } else {
          console.log("Game Over");
        }
        return true;
      }
    }

    return false;
  }

  eat() {
    const head = { ...this.body[0] };
    switch (this.direction) {
      case DIR.UP:
        head.y -= SNAKE_SIZE;
        break;
      case DIR.DOWN:
        head.y += SNAKE_SIZE;
        break;
      case DIR.LEFT:
        head.x -= SNAKE_SIZE;
        break;
      case DIR.RIGHT:
        head.x += SNAKE_SIZE;
        break;
    }
    this.body.unshift(head);
  }
}

const drawRemote = (body) => {
  body && localSnake.updateRemotePlayerBody(body);

  const CLASS = "remote";

  const oldElements = document.querySelectorAll("." + CLASS);

  oldElements.forEach((el) => {
    el.remove();
  });

  body.forEach((part) => {
    const snake = document.createElement("div");
    snake.classList.add(CLASS);
    snake.style.backgroundColor = "blue";
    snake.style.position = "absolute";
    snake.style.height = `${SNAKE_SIZE}px`;
    snake.style.width = `${SNAKE_SIZE}px`;
    snake.style.top = `${part.y}px`;
    snake.style.left = `${part.x}px`;
    snake.style.zIndex = 10;
    BOARD.appendChild(snake);
  });
};

const GAME_ID = "7I0SCU";
const IP = "192.168.1.100:5015"; //91.227.2.183:85
let remote_snake = [];
let localSnake = null;

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://" + IP + "/game")
  .configureLogging(signalR.LogLevel.Information)
  .build();

const loop = async (localSnake) => {
  localSnake.draw();
  localSnake.move();
  localSnake.updateRemotePlayerBody();

  setTimeout(loop, 150, localSnake);
};

async function start() {
  try {
    await connection.start();
    console.log("SignalR Connected.");

    connection.invoke("Move", GAME_ID, 1, { x: 100, y: 100, head: true });

    localSnake = new Snake(
      "red",
      false,
      true,
      remote_snake,
      connection,
      GAME_ID
    );

    loop(localSnake);
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000);
  }
}

connection.onclose(async () => {
  await start();
});

connection.on("SEND", (remotePlayer, game) => {
  // console.log(remotePlayer, game);
  drawRemote(remotePlayer.opponent.positions);
});

connection.on("GET", (obj) => {
  console.log(obj);
});

// Start the connection.
start();
