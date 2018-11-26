<template>
    <div>
        <intro></intro>
        <div class="mg-top">
            <v-layout row>
                <v-flex xs12 md8 offset-md2>
                    <div class="text-md-center">
                        <template v-for="(item, ix) in quicklinks">
                            <v-btn v-if="!item.autoHide || (item.authRequired && loggedIn)"
                                   :color="item.color"
                                   @click.native="item.router != null ? visitRoute(item.router) : visitUrl(item.link)">
                                {{ item.name }}
                                <v-icon right>{{ item.icon }}</v-icon>
                            </v-btn>
                        </template>
                    </div>
                </v-flex>
            </v-layout>
        </div>
    </div>
</template>

<script>
    import intro from '../../components/introAdmin.vue'

    export default {
        data() {
            return {
                quicklinks: [
                    {
                        name: 'Groups',
                        icon: 'work',
                        router: '/groups',
                        autoHide: true,
                        authRequired: true,
                        color: 'warning'
                    },
                    {
                        name: 'File categories',
                        icon: 'polymer',
                        router: '/filecategories',
                        autoHide: true,
                        authRequired: true,
                        color: 'lime'
                    },
                    {
                        name: 'Logs',
                        icon: 'schedule',
                        router: '/activitylogs',
                        autoHide: true,
                        authRequired: true,
                        color: 'teal'
                    },                   
                    {
                        name: 'Users',
                        icon: 'people',
                        router: '/users',
                        autoHide: true,
                        authRequired: true,
                        color: 'info'
                    },
                    {
                        name: 'Add user',
                        icon: 'person add',
                        router: '/register',
                        autoHide: true,
                        authRequired: true,
                        color: 'primary'
                    }
                ]
            }
        },
        computed: {
            loggedIn: function () {
                return this.$store.state.auth.loggedIn
            }
        },
        methods: {
            visitUrl(u) {
                window.open(u)
            },
            visitRoute(r) {
                this.$router.push(r)
            }
        },
        components: { intro }
    }
</script>

<style>
    .title-subcontent {
        margin-top: 2%;
    }
</style>
