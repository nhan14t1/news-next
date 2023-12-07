import NoLayout from "../../components/Layout/NoLayout";
import AdminLogin from "../../components/admin/login/AdminLogin";

const AdminLoginPage = () => {
  return <AdminLogin />
}

AdminLoginPage.getLayout = function(page) {
  return <NoLayout>{page}</NoLayout>
}

export default AdminLoginPage;