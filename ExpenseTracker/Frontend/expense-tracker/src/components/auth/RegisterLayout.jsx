export default function AuthLayout({ 
  children, 
  title,
  subtitle,
  variant = 'blue' 
}) {
  const variants = {
    emerald: 'from-emerald-50 via-teal-50 to-cyan-50',
    violet: 'from-violet-50 via-purple-50 to-fuchsia-50',
    blue: 'from-blue-50 via-indigo-50 to-purple-50',
  };

  const decorations = {
    emerald: {
      top: 'bg-emerald-200/30',
      bottom: 'bg-cyan-200/30',
    },
    violet: {
      top: 'bg-violet-200/30',
      bottom: 'bg-fuchsia-200/30',
    },
    blue: {
      top: 'bg-blue-200/30',
      bottom: 'bg-blue-100/60',
    },
  };

  return (
    <div className={`reg-layout bg-gradient-to-br ${variants[variant]}`}>
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className={`absolute top-0 right-0 w-69 h-96 ${decorations[variant].top} rounded-full blur-3xl`} />
        <div className={`absolute bottom-0 left-0 w-96 h-96 ${decorations[variant].bottom} rounded-full blur-3xl`} />
      </div>

      <div className="relative w-full max-w-xl">
        {(title || subtitle) && (
          <div className="text-center mb-8">
            {title && (
              <h1 className="text-4xl font-bold text-gray-900 mb-2">
                {title}
              </h1>
            )}
            {subtitle && (
              <p className="text-gray-600">{subtitle}</p>
            )}
          </div>
        )}
        <div className="reg-card">
          {children}
        </div>
      </div>
    </div>
  );
}