<template>
    <div>
        <div class="text-md-center">
            <v-card>
                <v-card-title>
                    File categories
                    <v-spacer></v-spacer>
                    <v-text-field append-icon="search"
                                  label="Search"
                                  single-line
                                  hide-details
                                  v-model="search"></v-text-field>
                </v-card-title>
                <v-data-table v-bind:headers="headers"
                              v-bind:items="items"
                              v-bind:search="search">
                    <template slot="items" slot-scope="props">
                        <td class="text-xs-right">{{ props.item.id }}</td>
                        <td class="text-xs-right">{{ props.item.name }}</td>
                        <td>
                            <v-btn color="primary"
                                   @click.native="editFileCategory( props.item.id )">edit</v-btn>
                        </td>
                        <v-btn round color="lime"
                               @click.native="categoryFiles( props.item.id )">
                            files
                            <v-icon right light>cloud_upload</v-icon>
                        </v-btn>
                    </template>
                    <template slot="pageText" slot-scope="{ pageStart, pageStop }">
                        From {{ pageStart }} to {{ pageStop }}
                    </template>
                </v-data-table>
            </v-card>

            <div class="text-xs-center">
                <v-btn outline color="indigo"
                       @click.native="addFileCategory()">Add new category</v-btn>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        data() {
            return {
                search: '',
                pagination: {},
                headers: [
                    { text: 'id', value: 'id' },
                    { text: 'name', value: 'name' },
                    { text: 'action', value: 'action' }
                ],
                items: [],
            }
        },

        computed: {
        },


        methods: {
            getFileCategories() {
                return new Promise((resolve, reject) => {

                    console.log('getFileCategories called.')

                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }

                    axios.get('/api/filecategories', config)
                        .then((res) => {
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })

            },
            editFileCategory: function (id) {
                console.log('editFileCategory called.' + id)
                this.$router.push({ name: 'filecategoryedit', params: { id: id } })
            },
            addFileCategory: function () {
                this.$router.push({ name: 'newfilecategory' })
            },
            categoryFiles: function (id) {
                console.log('categoryFiles called.' + id)
                this.$router.push({ name: 'categoryfiles', params: { id: id } })
            },
        },

        created() {
            this.getFileCategories()
        }
    }
</script>
