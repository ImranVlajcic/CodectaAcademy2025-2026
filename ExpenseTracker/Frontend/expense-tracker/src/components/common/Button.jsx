import { Loader2 } from 'lucide-react';

export default function Button({ 
  children, 
  variant = 'primary', 
  loading = false,
  disabled = false,
  type = 'button',
  icon: Icon,
  ...props 
}) {
  const variants = {
    primary: 'btn-primary',
    secondary: 'btn-secondary',
    danger: 'btn-danger',
    ghost: 'btn-ghost',
    add : 'btn-add'
  };

  return (
    <button
      type={type}
      disabled={loading || disabled}
      className={`${variants[variant]} ${loading || disabled ? 'opacity-50 cursor-not-allowed' : ''}`}
      {...props}
    >
      {loading ? (
        <>
          <Loader2 className="w-5 h-5 animate-spin" />
          <span>Loading...</span>
        </>
      ) : (
        <>
          {children}
          {Icon && <Icon className="w-5 h-5 group-hover:translate-x-1 transition-transform" />}
        </>
      )}
    </button>
  );
}