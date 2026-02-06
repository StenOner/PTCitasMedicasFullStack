const LogoutIcon = ({ size = 24, color = "currentColor" }) => (
  <svg
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
  >
    <path d="M9 3h-4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h4" />

    <path d="M16 17l5-5-5-5" />
    <path d="M21 12H9" />
  </svg>
)

export { LogoutIcon }