import AdminLayout from "../../../components/admin/AdminLayout";

function PostManagement() {
    return <>Post</>;
}

PostManagement.getLayout = function(page) {
    return <AdminLayout>{page}</AdminLayout>;
}

export default PostManagement;