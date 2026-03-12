export default function Input({ 
  label,
  error,
  icon: Icon,
  ...props 
}) {
  return (
    <div className="space-y-2">
      {label && (
        <label htmlFor={props.id} className="block text-sm font-semibold text-gray-700">
          {label}
        </label>
      )}
      
      <div className="relative">
        {Icon && (
          <div className="absolute left-3 top-1/2 transform -translate-y-1/2">
            <Icon className="w-5 h-5 text-gray-400" />
          </div>
        )}
        
        <input
          className={`input-field ${Icon ? 'pl-10' : ''} ${
            error ? 'border-red-300 focus:ring-red-500' : ''
          }`}
          {...props}
        />
      </div>
      
      {error && (
        <p className="text-sm text-red-600">{error}</p>
      )}
    </div>
  );
}