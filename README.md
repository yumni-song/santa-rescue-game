# 산타 구출 게임 (Santa Rescue Game)

## 프로젝트 소개

이 프로젝트는 Unity Engine을 기반으로 개발된 액션 어드벤처 게임인 '산타 구출 게임'의 기술 문서입니다. 'santa-rescue-game'이라는 저장소 이름과 내부 스크립트들을 분석한 결과, 플레이어는 잃어버린 선물(`FieldItem_Present.prefab`)이나 핵심 아이템(`ItemKeyEft.cs` 등으로 추정)을 수집하고, 다양한 적들과 상호작용하며 최종적으로 산타를 구출하거나(`SantaRescue.cs` 추정) 특정 탈출 지점(`SledGoalTrigger.cs` 추정)에 도달하는 것을 목표로 하는 것으로 추정됩니다.

본 프로젝트는 Unity 및 C#을 활용한 게임 개발 역량을 보여주기 위한 포트폴리오 목적으로 작성되었습니다.

## 주요 기능

제공된 정보와 핵심 파일 분석을 바탕으로 추정되는 주요 기능은 다음과 같습니다:

*   **플레이어 제어 및 액션:**
    *   걷기, 달리기, 점프 등 기본적인 플레이어 이동(`PlayerMovement.cs`).
    *   플레이어 생명력(HP) 관리, 데미지 적용, 회복, 사망 처리(`PlayerHealth.cs`).
    *   (추정) 눈덩이 던지기(`SnowballThrower.cs`, `SnowballProjectile.cs`) 등의 공격 액션.
*   **아이템 시스템:**
    *   인벤토리 시스템을 통한 아이템 획득, 저장, 관리(`Inventory.cs`).
    *   필드에 아이템을 배치하고 스폰하는 기능(`ItemDatabase.cs`).
    *   (추정) 폭탄(`ItemBombEft.cs`), 열쇠(`ItemKeyEft.cs`) 등 다양한 아이템 사용 및 효과.
*   **적(Enemy) 시스템:**
    *   모든 적 캐릭터의 기본 속성 및 동작(`EnemyBase.cs`).
    *   적의 HP, 공격력 관리 및 사망 시 이펙트/사운드 처리.
    *   (추정) 고스트 보스(`GhostBoss.cs`), 마녀 보스(`WitchBoss.cs`) 등 특정 보스 적들의 등장과 고유 패턴.
*   **게임 관리 시스템:**
    *   게임 내 씬(Scene) 전환 및 재시작(`SceneController.cs`).
    *   게임 플레이 시간 관리 및 시간 초과 시 게임 오버 처리(`TimeManager.cs`).
    *   배경 음악(BGM) 및 효과음(SFX)을 전역적으로 관리(`AudioManager.cs`).
*   **사용자 인터페이스 (UI):**
    *   플레이어의 현재 HP를 하트 아이콘으로 시각화(`PlayerHealthUI.cs`).
    *   남은 게임 시간을 표시하는 타이머 UI(`TimeManager.cs`).
    *   (추정) 인벤토리 상태, 아이템 정보 등을 표시하는 UI.

## 프로젝트 구조

프로젝트의 상세한 디렉토리 구조 정보는 제공되지 않았습니다. 하지만, Unity 프로젝트의 일반적인 관례와 제공된 핵심 파일 경로(`Assets/Scripts/...`)를 미루어 볼 때, `Assets` 폴더 내에 `Scripts`, `Prefabs`, `Scenes`, `Materials`, `Textures`, `Audio` 등의 표준적인 디렉토리 구조를 가질 것으로 추정됩니다.

특히, 대부분의 핵심 게임 로직은 `Assets/Scripts` 디렉토리 내에 C# 스크립트로 구현되어 있습니다.

## 핵심 파일 설명

*   [`Assets/Scripts/Player/PlayerMovement.cs`](Assets/Scripts/Player/PlayerMovement.cs)
    플레이어 캐릭터의 움직임(걷기, 달리기, 점프, 중력 적용)을 제어하는 스크립트입니다. `CharacterController` 컴포넌트와 함께 작동하며, 플레이어의 움직임을 일시적으로 정지시키는 `FreezePlayer()` 메서드를 포함합니다.
*   [`Assets/Scripts/Player/PlayerHealth.cs`](Assets/Scripts/Player/PlayerHealth.cs)
    플레이어의 생명력(HP)을 관리하는 핵심 스크립트입니다. HP 초기화, 데미지 적용, 회복, 피격 무적 시간, 그리고 사망 처리(`Die()` 메서드를 통한 게임 오버 및 씬 전환) 로직을 담당합니다. 씬 전환 시에도 유지되는 싱글톤 패턴으로 구현되었습니다.
*   [`Assets/Scripts/Enemy/EnemyBase.cs`](Assets/Scripts/Enemy/EnemyBase.cs)
    모든 적 캐릭터의 기본 동작과 속성(HP, 공격력, 사망 이펙트/사운드)을 정의하는 추상화된 베이스 스크립트입니다. 플레이어 보호막(`PlayerShield`)과의 상호작용 로직(이동 방해, 공격 가능 여부)도 포함되어 있습니다.
*   [`Assets/Scripts/Scene/SceneController.cs`](Assets/Scripts/Scene/SceneController.cs)
    게임 내 씬 전환을 담당하는 스크립트입니다. 메인 게임 씬, 시작 씬 로드 및 현재 씬 재시작 기능을 제공하며, 주로 UI 버튼 이벤트에 연결되어 사용됩니다.
*   [`Assets/Scripts/Item/Inventory.cs`](Assets/Scripts/Item/Inventory.cs)
    플레이어가 획득한 아이템들을 관리하는 인벤토리 시스템 스크립트입니다. 아이템 추가/제거, 인벤토리 슬롯 수 제한 기능을 제공하며, 필드 아이템과의 충돌 감지(`OnTriggerEnter`)를 통해 아이템을 획득하는 로직을 구현합니다. 싱글톤 패턴이 적용되었습니다.
*   [`Assets/Scripts/Item/ItemDatabase.cs`](Assets/Scripts/Item/ItemDatabase.cs)
    게임 내 필드에 배치될 아이템들의 데이터베이스를 정의하고, 실제 필드에 아이템을 스폰하는 역할을 수행합니다. 고정된 위치와 랜덤 위치에 아이템을 배치하는 복합적인 스폰 로직을 가지고 있습니다.
*   [`Assets/Scripts/AudioManager.cs`](Assets/Scripts/AudioManager.cs)
    게임의 배경 음악(BGM)과 효과음을 전역적으로 관리하는 싱글톤 스크립트입니다. 씬 전환 시 BGM을 제어하는 기능을 포함하여, 게임의 전체적인 사운드 경험을 담당합니다.
*   [`Assets/Scripts/TimeManager.cs`](Assets/Scripts/TimeManager.cs)
    게임 플레이 시간을 관리하는 싱글톤 스크립트입니다. 설정된 `totalTime`이 0이 되면 'BadEnd' 씬으로 전환하여 게임 오버를 처리합니다. UI 텍스트(`timerText`)를 통해 남은 시간을 표시합니다.
*   [`Assets/Scripts/UI/PlayerHealthUI.cs`](Assets/Scripts/UI/PlayerHealthUI.cs)
    플레이어의 현재 HP 상태를 UI에 하트 아이콘으로 시각화하는 스크립트입니다. `PlayerHealth` 스크립트와 연동하여 HP 변화에 따라 하트의 개수와 모양을 업데이트하며, 피격 시 하트 깜빡임 효과를 제공합니다.
*   [`Packages/manifest.json`](Packages/manifest.json)
    이 Unity 프로젝트가 의존하는 모든 Unity 패키지 및 해당 버전 정보를 정의하는 파일입니다. 프로젝트의 기술 스택 및 외부 라이브러리 사용 현황을 파악하는 데 활용됩니다.

## 기술 스택

### Frontend
*   **Unity Engine**: 인터랙티브한 2D/3D 게임을 신속하게 개발할 수 있는 포괄적인 툴과 에셋 관리 기능을 제공하여 초보자 포트폴리오에 적합합니다.
*   **C#**: 복잡한 게임 로직과 동작을 구현하기 위한 견고하고 객체 지향적인 언어를 제공하여 강력한 프로그래밍 기반을 보여줍니다.
*   **Unity UI (UGUI) 2.0.0**: 게임 환경 내에서 유연하고 동적인 인터랙티브 사용자 인터페이스를 생성할 수 있게 하여 UI/UX 구현 기술을 시연합니다.
*   **TextMesh Pro**: 인게임 디스플레이를 위한 풍부한 스타일링 옵션을 갖춘 고품질, 고성능 텍스트 렌더링을 제공하여 시각적 표현을 향상시킵니다.

### Backend
*   해당 없음

### Database
*   **Unity Scriptable Objects / Prefabs / Scenes**: 외부 데이터베이스 없이 게임 에셋과 그 속성을 정의하고 관리하는 통합적이고 직접적인 방법을 제공하여 효율적인 게임 데이터 관리를 보여줍니다.

### DevOps
*   **Git**: 협업 개발을 용이하게 하고, 변경 사항을 추적하며, 이전 상태로 쉽게 롤백할 수 있게 하여 전문적인 버전 제어 관행을 보여줍니다.
*   **GitHub**: 코드 공유, 이슈 추적 및 프로젝트 관리를 위한 중앙 집중식 플랫폼을 제공하여 개발 워크플로우에 대한 참여를 강조합니다.

## 시스템 아키텍처

이 프로젝트는 Unity Engine을 기반으로 하는 단일 플레이어 클라이언트 측 게임입니다. 모든 게임 로직과 데이터 관리는 C# 스크립트와 Unity 에셋(프리팹, 씬, 스크립터블 오브젝트) 내에서 이루어집니다.

플레이어의 움직임, 체력, 인벤토리, 적 AI, 아이템 스폰, 씬 전환, 오디오 및 타이머와 같은 핵심 게임 시스템은 C# 스크립트로 구현됩니다. `PlayerHealth`, `Inventory`, `AudioManager`, `TimerManager`와 같은 핵심 관리자들은 'DontDestroyOnLoad'와 싱글톤 패턴을 사용하여 씬 전환 시에도 상태를 유지하고 중앙 집중식으로 관리됩니다. UI는 Unity UI(UGUI)와 TextMesh Pro를 활용하여 게임 내 정보를 표시하고 상호작용을 처리합니다.

전통적인 백엔드나 외부 데이터베이스는 사용되지 않으며, 모든 게임 데이터는 클라이언트 번들 내부에 포함됩니다. 개발 및 버전 관리는 Git과 GitHub를 통해 이루어집니다.

```mermaid
graph TD
    classDef backend fill:#D4E6F1,stroke:#3498DB,stroke-width:2px;
    classDef external fill:#FADBD8,stroke:#E74C3C,stroke-width:2px;
    classDef storage fill:#D1F2EB,stroke:#2ECC71,stroke-width:2px;
    classDef user fill:#FCF3CF,stroke:#F1C40F,stroke-width:2px;
    classDef frontend fill:#E8F8F5,stroke:#1ABC9C,stroke-width:2px;
    classDef devops fill:#D6EAF8,stroke:#5DADE2,stroke-width:2px;

    A[User]:::user

    subgraph "Unity Game Client"
        B[Unity Game Client Runtime]:::frontend
        C[Player Input & Actions]:::frontend
        D[Player Logic (C#)]:::frontend
        E[Enemy Logic (C#)]:::frontend
        F[Item System (C#)]:::frontend
        G[Scene Manager (C#)]:::frontend
        H[UI Manager (C#)]:::frontend
        I[Audio Manager (C#)]:::frontend
        J[Timer Manager (C#)]:::frontend
    end

    subgraph "Game Data & Assets"
        K[Game Assets (Prefabs, Models, Textures)]:::storage
        L[Unity Scenes (.unity)]:::storage
        M[Game Data (C# Lists, ScriptableObjects)]:::storage
    end

    subgraph "Development & Version Control"
        N[C# Source Code]:::storage
        O[GitHub Repository]:::devops
    end

    A -- "Provides Input" --> C
    C -- "Sends Commands" --> D
    D -- "Interacts With" --> E
    D -- "Collects/Uses Item" --> F
    D -- "Updates UI State" --> H
    D -- "Triggers Game Event" --> G
    E -- "Inflicts Damage" --> D
    E -- "Checks Player Shield" --> D
    F -- "Manages Inventory State" --> H
    F -- "Heals Player" --> D
    G -- "Loads/Unloads Scenes" --> B
    G -- "Notifies Game State" --> J
    H -- "Renders UI Elements" --> B
    H -- "Receives UI Events" --> D
    I -- "Plays Game Audio" --> B
    J -- "Updates Time Display" --> H
    J -- "Triggers Bad Ending" --> G

    B -- "Uses Compiled Assets" --> K
    B -- "Loads Scene Definitions" --> L
    B -- "Executes C# Game Logic" --> N

    F -- "Manages Item Definitions" --> M
    N -- "Implements Player Logic" --> D
    N -- "Implements Enemy AI" --> E
    N -- "Implements Item Mechanics" --> F
    N -- "Implements Scene Flow" --> G
    N -- "Implements UI Logic" --> H
    N -- "Implements Audio Control" --> I
    N -- "Implements Timer Control" --> J

    O -- "Stores Versioned Source Code" --> N
    O -- "Stores Versioned Game Assets" --> K
    O -- "Stores Versioned Scene Files" --> L
```

## 실행 방법

`BuildFile/` 디렉토리에 대한 정보가 부족하며, 빌드 스크립트나 실행 파일이 제공되지 않아 정확한 실행 방법을 명시하기 어렵습니다. 프로젝트를 실행하려면 다음과 같은 일반적인 Unity 프로젝트 빌드 절차가 필요할 것으로 추정됩니다:

1.  **Unity Hub 및 Unity Editor 설치:** 프로젝트 버전에 맞는 Unity Editor (예: `Packages/manifest.json`에 명시된 버전)를 설치합니다.
2.  **프로젝트 클론:** GitHub 저장소를 로컬 환경으로 클론합니다.
    ```bash
    git clone https://github.com/yumni-song/santa-rescue-game.git
    ```
3.  **Unity Editor에서 프로젝트 열기:** Unity Hub를 통해 클론한 프로젝트 폴더를 열거나, Unity Editor에서 'Open Project'를 선택하여 해당 폴더를 지정합니다.
4.  **씬 로드 및 플레이:** Unity Editor에서 `Assets/Scenes` 폴더 (추정) 내의 시작 씬(예: `StartScene.unity` 또는 `MainScene.unity`로 추정)을 로드한 후, Unity Editor의 Play 버튼을 눌러 게임을 실행합니다.
5.  **독립 실행형 빌드:** `File > Build Settings` 메뉴를 통해 원하는 플랫폼(Windows, macOS 등)으로 빌드하여 독립 실행형 게임으로 만들 수 있습니다.

**추가 작성 필요:** 만약 빌드된 파일이나 구체적인 실행 지침이 있다면 여기에 추가되어야 합니다.

## 기술 선택 이유

*   **Unity Engine:** 2D/3D 게임을 신속하게 개발할 수 있는 통합 개발 환경과 강력한 에셋 관리 시스템을 제공하여, 특히 초보 개발자도 아이디어를 빠르게 구현하고 시각화할 수 있도록 돕습니다.
*   **C#:** 객체 지향 프로그래밍 패러다임을 지원하는 견고하고 안정적인 언어로, 복잡한 게임 로직과 시스템을 효율적으로 구현하고 유지보수성을 높이는 데 유리합니다.
*   **Unity UI (UGUI) / TextMesh Pro:** 게임 내에서 직관적이고 반응성 있는 사용자 인터페이스를 빠르게 구축할 수 있게 하며, TextMesh Pro를 통해 고품질의 텍스트 렌더링으로 사용자 경험을 향상시킵니다.
*   **Unity Scriptable Objects / Prefabs / Scenes:** 외부 데이터베이스 없이 게임 에셋과 데이터를 직접적이고 통합적으로 관리하는 효율적인 방법을 제공하여, 게임 개발 워크플로우를 단순화하고 프로젝트의 배포 편의성을 높입니다.
*   **Git / GitHub:** 코드 버전 관리와 협업을 위한 표준 도구로서, 개발 진행 상황을 체계적으로 관리하고, 변경 이력을 추적하며, 다른 개발자와의 협업 과정을 원활하게 합니다.

## 개선 방향

현재 분석 결과와 불확실한 부분을 고려하여 다음과 같은 개선 방향을 제안합니다.

1.  **명확한 프로젝트 목표 및 스토리라인 정의:**
    *   현재 추정되는 '산타 구출' 또는 '선물 수집' 목표를 명확히 정의하고, 게임의 배경 스토리와 엔딩 시나리오를 구체화하여 플레이어에게 더욱 몰입감 있는 경험을 제공합니다.
2.  **플레이어 액션 및 아이템 사용 메커니즘 확장:**
    *   `SnowballThrower.cs` 등으로 추정되는 공격 방식의 구체적인 구현과, `ItemBombEft.cs`, `ItemKeyEft.cs` 등으로 추정되는 아이템들의 인게임 사용 효과 및 상호작용 로직을 명확히 정의하고 확장하여 전략적인 플레이를 유도합니다.
3.  **적 AI 및 보스 패턴 다양화:**
    *   `EnemyBase.cs`를 기반으로 다양한 적 캐릭터를 추가하고, `GhostBoss.cs`, `WitchBoss.cs`와 같은 보스 적들에게 고유하고 도전적인 공격 패턴 및 약점을 부여하여 게임의 재미와 난이도를 높입니다.
4.  **사용자 경험(UX) 개선:**
    *   튜토리얼 또는 온보딩 시스템을 추가하여 신규 플레이어가 게임 규칙과 조작법을 쉽게 익힐 수 있도록 돕습니다.
    *   인벤토리 UI, 아이템 정보 표시, 게임 오버/승리 화면 등 전반적인 UI/UX를 개선하고 시각적 일관성을 강화합니다.
5.  **코드 리팩토링 및 문서화:**
    *   현재 싱글톤 패턴과 DontDestroyOnLoad가 많이 사용되는데, 이는 씬 간 데이터 유지가 용이하지만, 과도할 경우 의존성 증가 및 테스트의 어려움으로 이어질 수 있습니다. 필요에 따라 Event-Driven Architecture나 Service Locator 패턴 등을 고려하여 모듈 간의 결합도를 낮출 수 있습니다.
    *   각 스크립트와 중요한 함수에 대한 주석을 상세히 추가하고, 외부 개발자가 쉽게 이해하고 기여할 수 있도록 기술 문서를 보강합니다.
6.  **성능 최적화:**
    *   게임 빌드 후 프레임 드랍, 메모리 사용량 등을 분석하여 불필요한 연산이나 리소스 로드를 최적화하고, 다양한 환경에서 안정적인 플레이를 보장합니다.
7.  **지속적인 콘텐츠 추가:**
    *   새로운 레벨, 적 유형, 아이템, 퍼즐 요소 등을 지속적으로 추가하여 플레이어가 탐험하고 즐길 수 있는 콘텐츠의 양을 늘립니다.
8.  **테스트 자동화 (장기적 관점):**
    *   주요 게임 로직과 시스템에 대한 유닛 테스트 및 통합 테스트를 도입하여, 변경 사항이 발생했을 때 기존 기능의 오류를 자동으로 감지하고 안정성을 확보합니다.