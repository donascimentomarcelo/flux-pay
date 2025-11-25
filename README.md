# Estrutura de pastas

.vscode/
├── launch.json
└── tasks.json

src/
├── Pay.Api/
├── Pay.Application/
├── Pay.Domain/
├── Pay.Infrastructure/
└── Pay.OutboxWorker/

# Debug – Executando com F5

Rodar a API com F5:

- escolha “.NET Launch API”

Rodar o Worker com F5:

- escolha “.NET Launch OutboxWorker”

Rodar ambos simultaneamente (mexendo na API e Worker junto):

- escolha “Run ALL (API + Worker)”

#### Ctrl+Shift+B → run-api

#### Ctrl+Shift+B → run-worker

#### Ctrl+Shift+B → docker-up

#### Ctrl+Shift+B → ef-migrations-add

#### flux-pay

/Payments.Api
/Payments.Application
/Payments.Domain
/Payments.Infrastructure
/Payments.OutboxWorker
/Payments.PaymentWorker

# Diagrama de sequencia

sequenceDiagram
autonumber

    participant C as Client
    participant API as Payment API
    participant DB as Database
    participant OB as Outbox Table
    participant OW as Outbox Worker
    participant K as Kafka
    participant PW as Payment Worker
    participant P as Provider (Mock)

    C->>API: POST /payments
    API->>DB: Insert Payment (status=PENDING)
    API->>OB: Insert Outbox Event (payment.created)
    API-->>C: 202 Accepted (PaymentId)

    loop Every X seconds
        OW->>OB: Read unprocessed events
        OW->>K: Publish payment.created
        OW->>OB: Mark event as processed
    end

    K-->>PW: Consume payment.created
    PW->>P: Process Payment (Pix/Card/Boleto)
    P-->>PW: Response (APPROVED / DECLINED)

    PW->>DB: Update Payment Status
    PW->>K: Publish payment.completed
