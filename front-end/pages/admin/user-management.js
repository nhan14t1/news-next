import AdminLayout from "../../components/admin/AdminLayout";

function UserManagement() {
    return <>User</>;
}

UserManagement.getLayout = function(page) {
    return <AdminLayout>{page}</AdminLayout>;
}

export default UserManagement;