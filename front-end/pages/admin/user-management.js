import Head from "next/head";
import AdminLayout from "../../components/admin/AdminLayout";
import { WEB_NAME } from "../../shared/constants/app-const";

function UserManagement() {
    return <>
        <Head>
            <title>{`Quản lý người dùng - ${WEB_NAME}`}</title>
        </Head>

        Works
    </>;
}

UserManagement.getLayout = function (page) {
    return <AdminLayout>{page}</AdminLayout>;
}

export default UserManagement;