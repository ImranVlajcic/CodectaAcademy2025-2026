import RegisterLayout from "../components/auth/RegisterLayout";
import RegisterForm from "../components/auth/RegisterForm";

export default function RegisterPage() {
  return(
    <RegisterLayout variant="blue">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-gray-900 mt-4 mb-2">
            Create Account
          </h1>
          <p className="text-gray-600">
            Join us to start tracking your expenses
          </p>
        </div>
  
        <RegisterForm />
      </RegisterLayout>
  );
}