import React, { useEffect, useState, type ReactNode } from "react"
import gdgLogo from "./assets/gdg-developer-logo.svg"

type IconName = "arrow" | "calendar" | "clock" | "discord" | "github" | "image" | "instagram" | "linkedin" | "location"

const iconPaths: Record<IconName, ReactNode> = {
  arrow: <path d="M5 12h14m-5-5 5 5-5 5" />,
  calendar: (
    <>
      <path d="M7 3v3m10-3v3M4 9h16M5 5h14a1 1 0 0 1 1 1v13a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V6a1 1 0 0 1 1-1Z" />
      <path d="M8 13h2m4 0h2m-8 4h2" />
    </>
  ),
  clock: (
    <>
      <circle cx="12" cy="12" r="9" />
      <path d="M12 7v5l3 2" />
    </>
  ),
  discord: (
    <>
      <path d="M8 8.5a10 10 0 0 1 8 0m-9.5 8c3.7 2 7.3 2 11 0" />
      <circle cx="9.5" cy="13" r=".8" fill="currentColor" stroke="none" />
      <circle cx="14.5" cy="13" r=".8" fill="currentColor" stroke="none" />
      <path d="M7 6.5C4.8 9.7 4.5 13 5 16.7l3 1.6m9-11.8c2.2 3.2 2.5 6.5 2 10.2l-3 1.6" />
    </>
  ),
  github: (
    <path d="M15 21v-3.5c.1-1-.4-1.8-1-2.3 3.3-.4 6.8-1.6 6.8-7.2 0-1.6-.6-2.9-1.5-3.9.2-.4.7-1.9-.1-3.8 0 0-1.2-.4-4 1.5a14 14 0 0 0-7.2 0C5.2-.1 4 .3 4 .3c-.8 1.9-.3 3.4-.1 3.8A5.6 5.6 0 0 0 2.4 8c0 5.6 3.5 6.8 6.8 7.2-.4.4-.8 1-.9 1.8-.8.4-2.8 1-4-1.2-.8-1.3-2.2-1.4-2.2-1.4" />
  ),
  image: (
    <>
      <rect x="3" y="4" width="18" height="16" rx="3" />
      <circle cx="9" cy="10" r="2" />
      <path d="m5 18 4.5-4 3 2.5 2.5-2 4 3.5" />
    </>
  ),
  instagram: (
    <>
      <rect x="3" y="3" width="18" height="18" rx="5" />
      <circle cx="12" cy="12" r="4" />
      <circle cx="17.5" cy="6.5" r=".8" fill="currentColor" stroke="none" />
    </>
  ),
  linkedin: (
    <>
      <rect x="3" y="3" width="18" height="18" rx="3" />
      <path d="M8 10v7m0-10v.1M12 17v-7m0 3c.6-2 4-2.2 4 1v3" />
    </>
  ),
  location: (
    <>
      <path d="M20 10c0 5-8 11-8 11S4 15 4 10a8 8 0 1 1 16 0Z" />
      <circle cx="12" cy="10" r="2.5" />
    </>
  ),
}

function Icon({
  name,
  size = "regular",
}: {
  name: IconName
  size?: "small" | "regular"
}) {
  return (
    <svg
      aria-hidden="true"
      className={`icon icon--${size}`}
      fill="none"
      viewBox="0 0 24 24"
      stroke="currentColor"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      {iconPaths[name]}
    </svg>
  )
}

function Link({
  children,
  className = "",
  href,
  label,
}: {
  children: ReactNode
  className?: string
  href: string
  label?: string
}) {
  return React.createElement(
    "a",
    { className, href, "aria-label": label },
    children,
  )
}

function Action({
  children,
  href,
  kind = "filled",
}: {
  children: ReactNode
  href: string
  kind?: "filled" | "outlined"
}) {
  return (
    <Link className={`action action--${kind}`} href={href}>
      {children}
    </Link>
  )
}

const eventDate = new Date("2026-05-09T13:00:00+03:00")

function useCountdown() {
  const getTime = () => Math.max(0, eventDate.getTime() - Date.now())
  const [remaining, setRemaining] = useState(getTime)

  useEffect(() => {
    const timer = window.setInterval(() => setRemaining(getTime()), 1000)
    return () => window.clearInterval(timer)
  }, [])

  return {
    days: Math.floor(remaining / 86_400_000),
    hours: Math.floor((remaining / 3_600_000) % 24),
    minutes: Math.floor((remaining / 60_000) % 60),
    seconds: Math.floor((remaining / 1000) % 60),
  }
}

function Logo({ footer = false }: { footer?: boolean }) {
  return (
    <Link
      className={`logo ${footer ? "logo--footer" : ""}`}
      href="#top"
      label="Ana sayfa"
    >
      <span className="logo-mark" aria-hidden="true">
        <i className="logo-stroke logo-stroke--blue" />
        <i className="logo-stroke logo-stroke--red" />
        <i className="logo-stroke logo-stroke--yellow" />
        <i className="logo-stroke logo-stroke--green" />
      </span>
      <span className="logo-copy">
        <strong>GDG on Campus</strong>
        <small>Sakarya</small>
      </span>
    </Link>
  )
}

const team = [
  { accent: "blue", name: "Arda Timuçin", role: "Kulüp Başkanı" },
  { accent: "red", name: "Neslihan", role: "Core Team" },
  { accent: "yellow", name: "Eda", role: "Sosyal Medya & İçerik" },
  { accent: "green", name: "Enes", role: "Full Stack Developer" },
  { accent: "red", name: "Canan", role: "Etkinlik Koordinatörü" },
  { accent: "blue", name: "Nisa", role: "UI/UX Designer" },
  { accent: "yellow", name: "Berkan", role: "Mobile Developer" },
]

function DeveloperMark() {
  return (
    <div className="hero-mark">
      <img src={gdgLogo} alt="Google Developer Groups logosu" />
    </div>
  )
}

function MemberCard({ member }: { member: typeof team[number] }) {
  return (
    <article className={`team-card team-card--${member.accent}`}>
      <div
        className="photo-placeholder"
        role="img"
        aria-label={`${member.name} için fotoğraf yer tutucusu`}
      >
        <span>
          <Icon name="image" />
        </span>
        <small>Fotoğraf yakında</small>
      </div>
      <div className="member-details">
        <div>
          <div className="title-small">{member.name}</div>
          <p>{member.role}</p>
        </div>
        <div className="member-socials">
          <Link href="#ekibimiz" label={`${member.name} GitHub profili`}>
            <Icon name="github" size="small" />
          </Link>
          <Link href="#ekibimiz" label={`${member.name} LinkedIn profili`}>
            <Icon name="linkedin" size="small" />
          </Link>
        </div>
      </div>
    </article>
  )
}

function CountdownCard() {
  const time = useCountdown()
  const values = [
    [time.days, "Gün"],
    [time.hours, "Saat"],
    [time.minutes, "Dakika"],
    [time.seconds, "Saniye"],
  ]

  return (
    <article className="bento-card countdown-card">
      <div className="card-kicker">
        <span className="pulse-dot" />
        YAKLAŞAN ETKİNLİK
      </div>
      <div className="countdown-copy">
        <div className="display-small">Google I/O Extended Sakarya</div>
        <p>İlk etkinliğimize kalan süre</p>
      </div>
      <div className="countdown" aria-label="Etkinliğe kalan süre">
        {values.map(([value, label]) => (
          <div className="countdown-item" key={label}>
            <strong>{String(value).padStart(2, "0")}</strong>
            <span>{label}</span>
          </div>
        ))}
      </div>
    </article>
  )
}

const events = [
  {
    accent: "blue",
    date: "09 MAYIS 2026",
    illustration: "io",
    location: "SAÜ Kültür ve Kongre Merkezi",
    time: "13:00",
    title: "Google I/O Extended Sakarya",
  },
  {
    accent: "red",
    date: "23 MAYIS 2026",
    illustration: "ai",
    location: "Teknoloji Fakültesi",
    time: "15:30",
    title: "Build with AI Workshop",
  },
  {
    accent: "yellow",
    date: "06 HAZİRAN 2026",
    illustration: "career",
    location: "Turgut Özal Kültür Merkezi",
    time: "14:00",
    title: "Kariyer Sohbetleri: Google",
  },
]

function EventVisual({ type }: { type: string }) {
  return (
    <div className={`event-visual event-visual--${type}`} aria-hidden="true">
      <div className="visual-grid" />
      <span className="visual-orb visual-orb--one" />
      <span className="visual-orb visual-orb--two" />
      <span className="visual-symbol">
        {type === "io" ? "</>" : type === "ai" ? "AI" : "↗"}
      </span>
    </div>
  )
}

function EventCard({ event }: { event: typeof events[number] }) {
  return (
    <article className="event-card">
      <EventVisual type={event.illustration} />
      <div className="event-body">
        <span className={`date-label date-label--${event.accent}`}>
          {event.date}
        </span>
        <div className="title-medium">{event.title}</div>
        <div className="event-meta">
          <span>
            <Icon name="clock" size="small" />
            {event.time}
          </span>
          <span>
            <Icon name="location" size="small" />
            {event.location}
          </span>
        </div>
        <Link
          className="calendar-action"
          href="#etkinlikler"
          label={`${event.title} etkinliğini takvime ekle`}
        >
          <span>Takvime ekle</span>
          <Icon name="calendar" size="small" />
        </Link>
      </div>
    </article>
  )
}

const projects = [
  {
    accent: "blue",
    description:
      "Kampüs içindeki etkinlikleri, öğrenci topluluklarını ve duyuruları tek bir akıllı akışta buluşturan açık kaynak platform.",
    eyebrow: "KAMPÜS TEKNOLOJİSİ",
    index: "01",
    tags: ["React", "TypeScript", "Firebase"],
    title: "Campus Connect",
  },
  {
    accent: "green",
    description:
      "Sakarya'nın toplu ulaşım verilerini erişilebilir ve anlaşılır hale getiren, topluluk odaklı akıllı şehir projesi.",
    eyebrow: "AKILLI ŞEHİR",
    index: "02",
    tags: ["C#", "Blazor", "PostgreSQL"],
    title: "Sakarya Açık Veri",
  },
]

function ProjectCard({ project }: { project: typeof projects[number] }) {
  return (
    <article className={`project-card project-card--${project.accent}`}>
      <div className="project-top">
        <span className="project-eyebrow">{project.eyebrow}</span>
        <span className="project-index">{project.index}</span>
      </div>
      <div>
        <div className="display-small">{project.title}</div>
        <p>{project.description}</p>
      </div>
      <div className="project-footer">
        <div className="tags">
          {project.tags.map((tag) => (
            <span key={tag}>{tag}</span>
          ))}
        </div>
        <Link
          className="round-link"
          href="#projeler"
          label={`${project.title} projesini incele`}
        >
          <Icon name="arrow" />
        </Link>
      </div>
    </article>
  )
}

function SiteHeader({ eventsPage = false }: { eventsPage?: boolean }) {
  const homeHref = eventsPage ? "index.html" : "#top"
  const sectionHref = (section: string) =>
    eventsPage ? `index.html#${section}` : `#${section}`

  return (
    <header className="topbar">
      <nav className="nav-shell" aria-label="Ana menü">
        <Logo />
        <div className="nav-links">
          <Link
            className={`nav-link ${eventsPage ? "" : "nav-link--active"}`}
            href={homeHref}
          >
            Ana Sayfa
          </Link>
          <Link className="nav-link" href={sectionHref("ekibimiz")}>
            Ekibimiz
          </Link>
          <Link
            className={`nav-link ${eventsPage ? "nav-link--active" : ""}`}
            href="index.html?page=events"
          >
            Etkinlikler
          </Link>
          <Link className="nav-link" href={sectionHref("projeler")}>
            Projeler
          </Link>
        </div>
        <Action href={sectionHref("katil")}>
          Bize Katıl <Icon name="arrow" size="small" />
        </Action>
      </nav>
    </header>
  )
}

function SiteFooter() {
  return (
    <footer className="footer" id="katil">
      <div className="footer-main">
        <div className="footer-brand">
          <Logo footer />
          <p>
            Teknolojiye ilgi duyan herkese açık,
            <br />
            öğrenci odaklı bir Google Developer topluluğu.
          </p>
          <div className="socials">
            {(["instagram", "linkedin", "github", "discord"] as IconName[]).map(
              (name) => (
                <Link
                  className="social-link"
                  href="#katil"
                  label={name}
                  key={name}
                >
                  <Icon name={name} size="small" />
                </Link>
              ),
            )}
          </div>
        </div>
        <div className="footer-links">
          <div>
            <strong>Keşfet</strong>
            <Link href="index.html">Ana Sayfa</Link>
            <Link href="index.html?page=events">Etkinlikler</Link>
            <Link href="index.html#projeler">Projeler</Link>
          </div>
          <div>
            <strong>Topluluk</strong>
            <Link href="index.html#ekibimiz">Ekibimiz</Link>
            <Link href="#katil">Bize Katıl</Link>
            <Link href="#katil">İletişim</Link>
          </div>
        </div>
      </div>
      <div className="footer-bottom">
        <span>© 2026 GDG on Campus Sakarya</span>
        <span>Öğren · Üret · Paylaş</span>
      </div>
    </footer>
  )
}

function EventsPage() {
  return (
    <div className="app-shell" id="top">
      <SiteHeader eventsPage />
      <main className="events-page">
        <section
          className="events-page-hero"
          aria-labelledby="events-page-title"
        >
          <div className="mesh mesh--blue" />
          <div className="mesh mesh--red" />
          <div className="mesh mesh--yellow" />
          <div className="mesh mesh--green" />
          <span className="section-kicker section-kicker--blue">
            ETKİNLİK TAKVİMİ
          </span>
          <div className="display-large" id="events-page-title">
            Birlikte öğren,
            <br />
            üret ve paylaş.
          </div>
          <p>
            Teknoloji dünyasını birlikte keşfettiğimiz workshop, söyleşi ve
            topluluk buluşmalarında yerini al.
          </p>
        </section>

        <section
          className="section events-page-content"
          aria-labelledby="upcoming-events-title"
        >
          <article className="featured-event">
            <div className="featured-event-date">
              <span>MAY</span>
              <strong>09</strong>
              <small>2026</small>
            </div>
            <div className="featured-event-copy">
              <span className="card-kicker">
                <span className="pulse-dot" />
                SIRADAKİ ETKİNLİK
              </span>
              <div className="display-small">Google I/O Extended Sakarya</div>
              <p>
                Google I/O duyurularını, yeni teknolojileri ve geliştirici
                deneyimlerini Sakarya topluluğuyla birlikte keşfediyoruz.
              </p>
              <div className="featured-event-meta">
                <span>
                  <Icon name="clock" size="small" />
                  13:00
                </span>
                <span>
                  <Icon name="location" size="small" />
                  SAÜ Kültür ve Kongre Merkezi
                </span>
              </div>
              <div className="featured-event-actions">
                <Action href="#etkinlik-listesi">
                  Detayları İncele <Icon name="arrow" size="small" />
                </Action>
                <Action href="#etkinlik-listesi" kind="outlined">
                  <Icon name="calendar" size="small" />
                  Takvime Ekle
                </Action>
              </div>
            </div>
            <EventVisual type="io" />
          </article>

          <div className="events-list-heading" id="etkinlik-listesi">
            <div>
              <span className="section-kicker section-kicker--red">
                PROGRAM
              </span>
              <div className="display-medium" id="upcoming-events-title">
                Yaklaşan Etkinlikler
              </div>
            </div>
            <div className="event-filters" aria-label="Etkinlik filtreleri">
              <Link
                className="event-filter event-filter--active"
                href="#etkinlik-listesi"
              >
                Tümü
              </Link>
              <Link className="event-filter" href="#etkinlik-listesi">
                Workshop
              </Link>
              <Link className="event-filter" href="#etkinlik-listesi">
                Söyleşi
              </Link>
              <Link className="event-filter" href="#etkinlik-listesi">
                Topluluk
              </Link>
            </div>
          </div>

          <div className="event-grid">
            {events.map((event) => (
              <EventCard event={event} key={event.title} />
            ))}
          </div>
        </section>

        <section className="events-community">
          <span className="section-kicker section-kicker--green">
            BİRLİKTE DAHA GÜÇLÜ
          </span>
          <div className="display-medium">
            Bir sonraki etkinlikte görüşelim.
          </div>
          <p>
            Yeni etkinliklerden haberdar olmak ve topluluğa katılmak için bize
            ulaş.
          </p>
          <Action href="index.html#katil">
            Topluluğa Katıl <Icon name="arrow" size="small" />
          </Action>
        </section>
      </main>
      <SiteFooter />
    </div>
  )
}

export default function App() {
  if (new URLSearchParams(window.location.search).get("page") === "events") {
    return <EventsPage />
  }

  return (
    <div className="app-shell" id="top">
      <SiteHeader />

      <main>
        <section
          className="hero"
          aria-labelledby="hero-title"
          onPointerMove={(event) => {
            const bounds = event.currentTarget.getBoundingClientRect()
            event.currentTarget.style.setProperty(
              "--pointer-x",
              `${event.clientX - bounds.left}px`,
            )
            event.currentTarget.style.setProperty(
              "--pointer-y",
              `${event.clientY - bounds.top}px`,
            )
          }}
        >
          <div className="mesh mesh--blue" />
          <div className="mesh mesh--red" />
          <div className="mesh mesh--yellow" />
          <div className="mesh mesh--green" />
          <div className="hero-content">
            <DeveloperMark />
            <div className="display-large hero-title" id="hero-title">
              <span className="hero-title--blue">GDG</span>{" "}
              <span className="hero-title--red">on</span>{" "}
              <span className="hero-title--yellow">Campus</span>{" "}
              <span className="hero-title--green">Sakarya</span>
            </div>
            <p className="hero-description">
              Merak eden, üreten ve paylaşan öğrencileri bir araya getiriyoruz.
              Geleceğin teknolojilerini birlikte keşfetmeye hazır mısın?
            </p>
            <div className="hero-actions">
              <Action href="#projeler">
                Projeleri İncele <Icon name="arrow" size="small" />
              </Action>
              <Action href="#etkinlikler" kind="outlined">
                Etkinliklere Göz At
              </Action>
            </div>
          </div>
          <div className="scroll-cue" aria-hidden="true">
            <span />
          </div>
        </section>

        <section className="section bento-section" aria-label="Topluluk özeti">
          <div className="bento-grid">
            <CountdownCard />
            <article className="bento-card member-card">
              <div className="member-icon">
                <span />
                <span />
                <span />
              </div>
              <div>
                <strong className="stat-number">
                  50<span>+</span>
                </strong>
                <div className="title-small">Aktif Üye</div>
                <p>Birlikte öğrenen, paylaşan ve üreten bir topluluk.</p>
              </div>
              <div className="member-growth">+18% bu dönem</div>
            </article>
            <article className="bento-card discord-card">
              <div className="discord-icon">
                <Icon name="discord" />
              </div>
              <div>
                <div className="title-small">
                  Sohbete katıl,
                  <br />
                  bağlantıda kal.
                </div>
                <p>Discord sunucumuzda yerini al.</p>
              </div>
              <Link className="discord-link" href="#katil">
                Sunucuya katıl <Icon name="arrow" size="small" />
              </Link>
            </article>
          </div>
        </section>

        <section
          className="section team-section"
          id="ekibimiz"
          aria-labelledby="team-title"
        >
          <div className="section-heading">
            <div>
              <span className="section-kicker section-kicker--red">
                BİZİ TANIYIN
              </span>
              <div className="display-medium" id="team-title">
                Ekibimiz
              </div>
            </div>
            <p>
              Farklı disiplinlerden, aynı meraktan.
              <br />
              Topluluğu birlikte büyütüyoruz.
            </p>
          </div>
          <div className="team-grid">
            {team.map((member) => (
              <MemberCard member={member} key={member.name} />
            ))}
          </div>
        </section>

        <section
          className="section events-section"
          id="etkinlikler"
          aria-labelledby="events-title"
        >
          <div className="section-heading">
            <div>
              <span className="section-kicker section-kicker--blue">
                BİRLİKTE ÖĞRENİYORUZ
              </span>
              <div className="display-medium" id="events-title">
                Etkinliklerimiz
              </div>
            </div>
            <Link className="text-link" href="#etkinlikler">
              Tüm etkinlikler <Icon name="arrow" size="small" />
            </Link>
          </div>
          <div className="event-grid">
            {events.map((event) => (
              <EventCard event={event} key={event.title} />
            ))}
          </div>
        </section>

        <section
          className="section projects-section"
          id="projeler"
          aria-labelledby="projects-title"
        >
          <div className="section-heading">
            <div>
              <span className="section-kicker section-kicker--green">
                FİKİRDEN ÜRÜNE
              </span>
              <div className="display-medium" id="projects-title">
                Kulüp Projeleri
              </div>
            </div>
            <p>
              Teknolojiyi gerçek sorunlara
              <br />
              çözüm üretmek için kullanıyoruz.
            </p>
          </div>
          <div className="project-grid">
            {projects.map((project) => (
              <ProjectCard project={project} key={project.title} />
            ))}
          </div>
        </section>
      </main>

      <SiteFooter />
    </div>
  )
}
