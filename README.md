# ♠️ RiverSight

> **Engenharia de Software aplicada à Teoria dos Jogos.**
> Um assistente de Poker (HUD) passivo baseado em Visão Computacional e análise estatística para emuladores Android.

## 📖 Sobre o Projeto

O **RiverSight** é um projeto de estudo em **C# (.NET 8)** focado na criação de um HUD (Heads-Up Display) para aplicativos de Poker móveis (como PPPoker e Suprema) rodando em emuladores no Windows.

Diferente de bots tradicionais que injetam código na memória do jogo (o que é inseguro e detectável), o RiverSight atua como um **"olho externo"**. Ele utiliza **Visão Computacional (OpenCV)** para "ler" a tela do emulador e tomar decisões matemáticas baseadas em Teoria dos Jogos (GTO/Exploitativo), sem jamais interagir diretamente com o processo do jogo.

### 🚀 Diferenciais Técnicos
* **Non-Intrusive:** Não realiza injeção de DLL ou leitura de memória (`ReadProcessMemory`). Atua apenas sobre pixels.
* **Domain-Driven Design (DDD):** Arquitetura desacoplada, separando a lógica de poker da implementação de visão computacional.
* **High Performance:** Otimizado para baixa latência usando `OpenCvSharp4` e `Template Matching`.

---

## 🏗️ Arquitetura (DDD)

A solução está estruturada seguindo os princípios de Domain-Driven Design para garantir manutenibilidade e escalabilidade.

```text
RiverSight.sln
│
├── 1. RiverSight.Domain (Core)
│   ├── Entidades puras (Card, Hand, Player)
│   ├── Value Objects imutáveis (Suit, Rank)
│   ├── Interfaces de Serviços (IScreenReader)
│   └── *Zero dependências externas.*
│
├── 2. RiverSight.Infra.Vision (Infrastructure)
│   ├── Implementação do OpenCV (OpenCvSharp4)
│   ├── Algoritmos de Template Matching
│   └── Captura de tela (Win32 API / GDI+)
│
├── 3. RiverSight.Application (Orchestration)
│   ├── Casos de Uso (AnalyzeHand, CalculateEquity)
│   └── DTOs
│
└── 4. UI (Presentation)
    ├── RiverSight.ConsoleTester (Para debug de visão)
    └── RiverSight.UI.Overlay (WPF Transparente - *Em breve*)
```
# 🛠️ Documentação Técnica & Setup

## 💻 Tech Stack

* **Linguagem:** C# (.NET 8)
* **Computer Vision:** OpenCvSharp4 (Wrapper do OpenCV)
* **Ambiente Alvo:** LDPlayer 9 / MuMu Player
* **IDE:** Visual Studio Enterprise 2025
---

## ⚙️ Configuração do Ambiente (Obrigatório)

Para que o motor de visão funcione com precisão de pixel, o emulador **DEVE** estar configurado rigorosamente como abaixo:

### 1. Emulador (LDPlayer 9)
* **Resolução:** Personalizada (**540** largura x **960** altura).
* **DPI:** 160.
* **Modo de Janela:** "Fixar tamanho da janela" (*Lock Window Size*) **ATIVADO**.
* **Orientação:** Modo Celular (*Portrait*).

### 2. Assets (Templates)
O sistema funciona comparando pixels (`Template Matching`). É necessário popular a pasta `Assets` com recortes das cartas do jogo específico:

1. Tire um print da mesa do emulador.
2. Recorte apenas a face da carta (sem bordas da mesa).
3. Salve como: `Ah.png` (Ás de Copas), `Ks.png` (Rei de Espadas), `Td.png` (Dez de Ouros), etc.

### 3. Windows
* Recomenda-se **desativar o Hyper-V** para garantir a performance e estabilidade do emulador Android (evitar conflito de virtualização VT-x).

---

## 🚦 Roadmap & Status

### Fase 1: A Visão (MVP) - *Atual*
- [x] Configuração da Arquitetura DDD.
- [x] Implementação do `CardParser` e Entidades.
- [x] Motor de Visão com `Template Matching` (OpenCV).
- [x] Leitura de Cartas do Hero (Mão).
- [ ] Leitura de Cartas Comunitárias (Board).

### Fase 2: O Cérebro (Matemática)
- [ ] Cálculo de Pot Odds.
- [ ] Integração com biblioteca de Equidade (Monte Carlo).
- [ ] Decisão básica (Fold/Call/Raise) baseada em matemática pura.

### Fase 3: A Interface
- [ ] Overlay WPF transparente (*Click-through*).
- [ ] Exibição de estatísticas em tempo real sobre a mesa.

---

## ⚠️ Aviso Legal

Este software é desenvolvido estritamente para fins **educacionais** e de pesquisa acadêmica em Engenharia de Software e Visão Computacional. O autor não encoraja o uso da ferramenta para violar Termos de Serviço de plataformas de jogos. O uso deste software é de inteira responsabilidade do usuário.

---

**Desenvolvido como projeto de estudo em C# e Arquitetura de Software.**
