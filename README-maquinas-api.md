# README - API de Máquinas

Este documento descreve a estrutura mínima recomendada para implementar a API REST de máquinas em C# ASP.NET Core para o mapa interativo. A lógica atual do front-end já trabalha com criação, edição, remoção e reposicionamento por coordenadas `x` e `y`, então o back-end deve refletir exatamente essas operações.[cite:21][cite:31]

## Objetivo

A API deve persistir as máquinas que hoje são gerenciadas pelo `MaquinaService` no front-end. Como o mapa usa SVG, coordenadas livres e arraste no plano, cada máquina precisa guardar sua própria posição e seus dados básicos, sem depender do layout visual para existir.[cite:8][cite:24]

## Entidade principal

A entidade principal pode se chamar `Maquina`. Ela representa cada item desenhado no mapa e precisa conter identificação, dados descritivos e posição no plano.[cite:21][cite:24]

### Campos sugeridos

| Campo | Tipo sugerido em C# | Obrigatório | Observação |
|------|------|------|------|
| `Id` | `Guid` ou `string` | Sim | Gerado no backend.[cite:25] |
| `Patrimonio` | `string` | Sim | Código/identificação do equipamento.[cite:21] |
| `Setor` | `string` | Sim | Ex.: TI, RH, Produção.[cite:21] |
| `Descricao` | `string?` | Não | Texto livre opcional.[cite:21] |
| `X` | `double` | Sim | Coordenada horizontal no mapa.[cite:24] |
| `Y` | `double` | Sim | Coordenada vertical no mapa.[cite:24] |
| `CriadoEm` | `DateTime` | Sim | Definido no backend.[cite:24] |
| `AtualizadoEm` | `DateTime` | Sim | Atualizado em edição ou movimento.[cite:24] |

## DTOs recomendados

Separar DTOs ajuda a manter a API simples e clara. Como o front-end já separa criação, edição e movimento, faz sentido refletir isso no backend.[cite:21][cite:24]

### CreateMaquinaRequest

Usado para criar uma nova máquina.

Campos:

- `Patrimonio`
- `Setor`
- `Descricao`
- `X`
- `Y`

### UpdateMaquinaRequest

Usado para editar os dados da máquina sem mexer na posição.

Campos:

- `Patrimonio`
- `Setor`
- `Descricao`

### MoveMaquinaRequest

Usado para atualizar apenas a posição após arraste no mapa.

Campos:

- `X`
- `Y`

### MaquinaResponse

Objeto retornado pela API.

Campos:

- `Id`
- `Patrimonio`
- `Setor`
- `Descricao`
- `X`
- `Y`
- `CriadoEm`
- `AtualizadoEm`

## Rotas da API

Estas rotas cobrem o comportamento atual do sistema de mapa: listar, criar, editar, mover e remover máquinas.[cite:21][cite:24]

| Método | Rota | Finalidade |
|------|------|------|
| `GET` | `/api/maquinas` | Lista todas as máquinas.[cite:21] |
| `GET` | `/api/maquinas/{id}` | Busca uma máquina por id.[cite:25] |
| `POST` | `/api/maquinas` | Cria uma máquina nova no mapa.[cite:21][cite:31] |
| `PUT` | `/api/maquinas/{id}` | Atualiza os dados da máquina.[cite:21] |
| `PATCH` | `/api/maquinas/{id}/posicao` | Atualiza somente `X` e `Y` após arraste.[cite:24] |
| `DELETE` | `/api/maquinas/{id}` | Remove a máquina.[cite:21] |

## Exemplos de payload

### Criar máquina

**POST** `/api/maquinas`

```json
{
  "patrimonio": "12345",
  "setor": "TI",
  "descricao": "Desktop Dell",
  "x": 1200,
  "y": 850
}
```

### Atualizar dados

**PUT** `/api/maquinas/{id}`

```json
{
  "patrimonio": "12345",
  "setor": "RH",
  "descricao": "Notebook Lenovo"
}
```

### Atualizar posição

**PATCH** `/api/maquinas/{id}/posicao`

```json
{
  "x": 1400,
  "y": 910
}
```

## Convenção de nomes em C#

Uma convenção simples e segura seria esta:

- Entidade: `Maquina`
- Controller: `MaquinasController`
- Service: `MaquinaService` ou `MaquinasService`
- DTOs:
  - `CreateMaquinaRequest`
  - `UpdateMaquinaRequest`
  - `MoveMaquinaRequest`
  - `MaquinaResponse`
- DbSet: `Maquinas`[cite:25]

## Regras básicas do backend

Estas regras já acompanham a lógica atual do front-end e evitam inconsistência de dados:[cite:21][cite:24]

- `Id` deve ser gerado no backend.
- `CriadoEm` deve ser preenchido no backend.
- `AtualizadoEm` deve ser atualizado sempre que a máquina for editada ou movida.
- `Patrimonio` deve ser obrigatório.
- `Setor` deve ser obrigatório.
- `X` e `Y` devem ser obrigatórios na criação.
- O endpoint de mover deve alterar apenas `X`, `Y` e `AtualizadoEm`.

## Estrutura mínima de arquivos

Uma organização simples em ASP.NET Core pode ser esta:

- `Controllers/MaquinasController.cs`
- `Models/Maquina.cs`
- `Dtos/CreateMaquinaRequest.cs`
- `Dtos/UpdateMaquinaRequest.cs`
- `Dtos/MoveMaquinaRequest.cs`
- `Dtos/MaquinaResponse.cs`
- `Data/AppDbContext.cs`
- `Services/MaquinaService.cs`[cite:25]

## Observação final

Como o mapa já usa SVG, `viewBox`, coordenadas livres e atualização reativa no front-end, a responsabilidade do backend não é desenhar nada; ele só precisa persistir corretamente os dados da máquina e devolver essas informações para o Angular renderizar no mapa.[cite:8][cite:13][cite:24]
