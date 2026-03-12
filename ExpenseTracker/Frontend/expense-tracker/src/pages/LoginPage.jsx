import AuthLayout from '../components/auth/AuthLayout';
import LoginForm from '../components/auth/LoginForm';

export default function LoginPage() {
  return (
    <AuthLayout variant="blue">
      <div className="text-center mb-8">
        <h1 className="text-4xl font-bold text-gray-900 mt-4 mb-2">
          Welcome Back
        </h1>
        <p className="text-gray-600">
          Sign in to manage your expenses
        </p>
      </div>

      <LoginForm />
    </AuthLayout>
  );
}