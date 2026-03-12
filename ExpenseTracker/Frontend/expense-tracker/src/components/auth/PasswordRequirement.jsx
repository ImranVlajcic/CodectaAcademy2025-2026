import { CheckCircle2 } from "lucide-react";

export default function PasswordRequirement({ met, text }) {
  return (
    <div className="flex items-center gap-2">
      <div className={`w-4 h-4 rounded-full flex items-center justify-center ${
        met ? 'bg-emerald-500' : 'bg-gray-300'
      }`}>
        {met && <CheckCircle2 className="w-3 h-3 text-white" />}
      </div>
      <span className={`text-xs ${met ? 'text-emerald-700' : 'text-gray-600'}`}>
        {text}
      </span>
    </div>
  );
}