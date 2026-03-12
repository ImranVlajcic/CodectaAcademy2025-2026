import useAuth from '../hooks/useAuth';
import DashboardLayout from '../components/common/DashboardLayout';
import UserProfile from '../components/common/UserProfile';

export default function UserProfilePage() {
  const {
    user,
    handleLogout,
  } = useAuth();

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
        <UserProfile>
        </UserProfile>
    </DashboardLayout>
  );
}